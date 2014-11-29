package main

import (
    "fmt"
    "net/http"
    "encoding/json"
    "github.com/gorilla/mux"
    "gopkg.in/mgo.v2"
    "gopkg.in/mgo.v2/bson"
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
    r := mux.NewRouter()

    r.HandleFunc("/feeds", getFeeds).Methods("GET")
    r.HandleFunc("/feeds", putFeeds).Methods("PUT")
    //r.HandleFunc("/feeds/items", emptyHandler).Methods("GET")
    //r.HandleFunc("/feeds/items/{id}/read", emptyHandler).Methods("POST")
    //r.HandleFunc("/feeds/items/{id}/star", emptyHandler).Methods("GET")
    //r.HandleFunc("/feeds/items/{id}/unstar", emptyHandler).Methods("GET")

    http.Handle("/", r)
    http.ListenAndServe(":3000", nil)
}

// 获取订阅列表
func getFeeds(w http.ResponseWriter, r *http.Request) {
  session := createSession()
  defer session.Close()
  c := session.DB("reader").C("users")
  user := User{}
  c.Find(bson.M{"name": "zackyang"}).One(&user)
  content, _ := json.Marshal(user)
  w.Write(content)
}

// 保存订阅列表
func putFeeds(w http.ResponseWriter, r *http.Request) {
  query := r.URL.Query()
  session := createSession()
  defer session.Close()
  c := session.DB("reader").C("users")
  c.UpdateId(query["_id"], query)
  fmt.Fprintf(w, "ok")
}

func emptyHandler(w http.ResponseWriter, r *http.Request) {
    params := mux.Vars(r)
    w.Header().Set("Content-Type", "application/json")
    content, _ := json.Marshal(params)
    w.Write(content)
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
