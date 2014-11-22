package main

import (
  "fmt"
  rss "github.com/jteeuwen/go-pkg-rss"
  "os"
)

func main() {
  PollFeed("http://zhihu.com/rss", 30)
}

func PollFeed(uri string, timeout int) {
  feed := rss.New(timeout, true, chanHandler, itemHandler)

  for {
    if err := feed.Fetch(uri, nil); err != nil {
      fmt.Fprintf(os.Stderr, "[e] %s: %s", uri, err)
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
