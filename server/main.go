package main

import (
	"fmt"
	"github.com/go-martini/martini"
	"github.com/martini-contrib/cors"
	"github.com/martini-contrib/render"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

type Resource struct {
	Url string
}

type Item struct {
	FeedLink    string // 所属RSS的订阅地址
	Title       string // 标题
	Description string // 正文
	Link        string // 链接
	Author      string // 作者
	Date        string // 发布日期
}

type User struct {
	Name    string // 用户名
	Folders struct {
		Name  string // 文件夹名, 若为空则为根目录
		Order int    // 文件夹排序
		Feeds struct {
			Url         string   // 订阅Url
			Name        string   // 用于自定义的订阅名
			Order       int      // 订阅排序
			UnreadItems []string // 未读条目的Link
		}
	}
}

func main() {
	m := martini.Classic()
	m.Use(render.Renderer())
	m.Use(cors.Allow(&cors.Options{
		AllowOrigins:     []string{"*"},
		AllowMethods:     []string{"*"},
		AllowHeaders:     []string{"Origin"},
		ExposeHeaders:    []string{"Content-Length"},
		AllowCredentials: true,
	}))

	// 获取订阅列表
	m.Get("/feeds", func(params martini.Params, r render.Render) {
		username := params["username"]
		session := createSession()
		defer session.Close()
		c := session.DB("reader").C("users")
		user := User{}
		c.Find(bson.M{"name": username}).One(&user)
		r.JSON(200, user)
	})

	// 保存订阅列表
	m.Put("/feeds", func(params martini.Params, r render.Render) {
		session := createSession()
		defer session.Close()
		c := session.DB("reader").C("users")
		c.UpdateId(params["_id"], params)
		r.JSON(200, map[string]interface{}{"result": "ok"})
	})

	m.RunOnAddr(":31001")
	m.Run()

	//TODO
	//GET /feeds/items
	//POST /feeds/items/{id}/read
	//POST /feeds/items/{id}/star
	//POST /feeds/items/{id}/unstar
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
