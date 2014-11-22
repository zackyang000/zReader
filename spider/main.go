/*package main

import (
"io/ioutil";
"net/http";
"fmt"
)

func main() {
  response, err := http.Get("http://zhihu.com/rss")
  if err != nil {
    fmt.Printf("Error: %s\n", err)
  } else {
    defer response.Body.Close()
    content, err := ioutil.ReadAll(response.Body)
    if err != nil {
      fmt.Printf("%s", err)
    } else {
        fmt.Printf("%s", string(content))
    }
  }
}
*/

package main

import (
  "fmt"
  rss "github.com/jteeuwen/go-pkg-rss"
  "os"
  "time"
)

func main() {
  PollFeed("http://blog.case.edu/news/feed.atom", 30)
}

func PollFeed(uri string, timeout int) {
  feed := rss.New(timeout, true, chanHandler, itemHandler)

  for {
    if err := feed.Fetch(uri, nil); err != nil {
      fmt.Fprintf(os.Stderr, "[e] %s: %s", uri, err)
      return
    }

    <-time.After(time.Duration(feed.SecondsTillUpdate()))
  }
}

func chanHandler(feed *rss.Feed, newchannels []*rss.Channel) {
  fmt.Printf("%d new channel(s) in %s\n", len(newchannels), feed.Url)
}

func itemHandler(feed *rss.Feed, ch *rss.Channel, newitems []*rss.Item) {
  fmt.Printf("%d new item(s) in %s\n", len(newitems), feed.Url)
}
