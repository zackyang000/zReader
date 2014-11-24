package main

import (
  "fmt"
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
  list := []Resource{
    Resource{ Url: "http://zhihu.com/rss" },
    Resource{ Url: "http://feed.zackyang.com/articles.xml" },
  }
  for _, resource := range list {
    feed := rss.New(5, true, chanHandler, itemHandler)
    if err := feed.Fetch(resource.Url, nil); err != nil {
      fmt.Printf("Error: %s - %s", resource.Url, err)
    }
  }
}

func chanHandler(feed *rss.Feed, newchannels []*rss.Channel) {
  fmt.Printf("%d channel(s) in %s\n", len(newchannels), feed.Url)
}

func itemHandler(feed *rss.Feed, ch *rss.Channel, newitems []*rss.Item) {
  fmt.Printf("%d new item(s) in %s\n", len(newitems), feed.Url)

  session, err := mgo.Dial("127.0.0.1")
  if err != nil {
    fmt.Printf("Error: mongoDB - %s", err)
  }
  defer session.Close()
  session.SetMode(mgo.Monotonic, true)
  c := session.DB("reader").C("items")
  for _, item := range newitems {
    err = c.Insert(item)
    if err != nil {
      fmt.Printf("Error: mongoDB - %s", err)
    }

    item := rss.Item{}
    err = c.Find(bson.M{"id": ""}).One(&item)
    if err != nil {
      fmt.Printf("Error: mongoDB - %s", err)
    }
    fmt.Println("Title:", item.Title)
  }
}
