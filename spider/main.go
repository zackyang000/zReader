package main

import (
  "fmt"
  "os"
  rss "github.com/jteeuwen/go-pkg-rss"
)

func main() {
  list := []string{
    "http://zhihu.com/rss",
  }
  for _, uri := range list {
    feed := rss.New(5, true, chanHandler, itemHandler)

    if err := feed.Fetch(uri, nil); err != nil {
      fmt.Printf("Get %s has been error: %s", uri, err)
    }
  }
}

func chanHandler(feed *rss.Feed, newchannels []*rss.Channel) {
  fmt.Printf("%d new channel(s) in %s\n", len(newchannels), feed.Url)
  for i := 0; i < len(newchannels); i++ {
    fmt.Printf("%s channel: %s\n", feed.Url, newchannels[i].Title)
  }
}

func itemHandler(feed *rss.Feed, ch *rss.Channel, newitems []*rss.Item) {
  fmt.Printf("%d new item(s) in %s\n", len(newitems), feed.Url)
  for i := 0; i < len(newitems); i++ {
    fmt.Printf("%s item: %s\n", feed.Url, newitems[i].Title)
  }
}
