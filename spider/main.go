package main

import (
  "fmt"
  "time"
  "gopkg.in/mgo.v2"
  "gopkg.in/mgo.v2/bson"
  rss "github.com/jteeuwen/go-pkg-rss"
)

type Resource struct {
  Url string
}

type Item struct {
  Title string
  Description string
  Link string
  Author string
  Date string
}

type User struct {
  Name string
  Folders struct {
    Name string
    Order int
    Feeds struct {
      Url string
      Name string
      Order int
    }
  }
}

func main() {
  /*
  resources := []Resource{
    Resource{ Url: "http://zhihu.com/rss" },
    Resource{ Url: "http://feed.zackyang.com/articles.xml" },
  }
  */
  for {
    resources := getResources()
    resources = []Resource{
      Resource{ Url: "http://zhihu.com/rss" },
      Resource{ Url: "http://feed.zackyang.com/articles.xml" },
    }
    // 获取所有订阅内容
    for _, resource := range resources {
      feed := rss.New(5, true, channelHandler, itemHandler)
      if err := feed.Fetch(resource.Url, nil); err != nil {
        fmt.Printf("Error: %s - %s", resource.Url, err)
      }
    }

    <-time.After(time.Minute * 10)
  }

}

// 获取所有订阅URL
func getResources() []Resource {
  session := createSession()
  defer session.Close()
  c := session.DB("reader").C("resource")
  resources := []Resource{}
  c.Find(bson.M{}).Iter().All(&resources)
  return resources
}

func channelHandler(feed *rss.Feed, newchannels []*rss.Channel) {
  //fmt.Printf("Get %d channel(s) in %s\n", len(newchannels), feed.Url)
}

func itemHandler(feed *rss.Feed, ch *rss.Channel, newitems []*rss.Item) {
  //fmt.Printf("Get %d new item(s) in %s\n", len(newitems), feed.Url)

  session := createSession()
  defer session.Close()
  c := session.DB("reader").C("items")

  for _, item := range newitems {
    item := Item{
      Title : item.Title,
      Description : item.Description,
      Link : item.Links[0].Href,
      Author : item.Author.Name,
      Date : item.PubDate,
    }

    //判断是否已存在
    count, _ := c.Find(bson.M{"link": item.Link}).Count()
    if count == 0 {
      err := c.Insert(item)
      if err != nil {
        fmt.Printf("Error: mongoDB - %s\n", err)
      }
      fmt.Printf("[%s][%s] %s\n", time.Now().Format("2000-01-01 00:00"), ch.Title, item.Title)
    }
  }
}

// 建立数据库连接
func createSession() *mgo.Session {
  session, err := mgo.Dial("127.0.0.1")
  if err != nil {
    fmt.Printf("Error: mongoDB - %s", err)
  }
  session.SetMode(mgo.Monotonic, true)
  return session
}
