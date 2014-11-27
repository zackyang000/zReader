package main

import (
    "fmt"
    "net/http"
)

func main() {
    http.HandleFunc("/", rootHandler)

    http.ListenAndServe(":8080", nil)
}


func rootHandler(w http.ResponseWriter, r *http.Request) {
    fmt.Fprintf(w, "rootHandler: %s\n", r.URL.Path)
    fmt.Fprintf(w, "URL: %s\n", r.URL)
    fmt.Fprintf(w, "Method: %s\n", r.Method)
    fmt.Fprintf(w, "RequestURI: %s\n", r.RequestURI )
    fmt.Fprintf(w, "Proto: %s\n", r.Proto)
    fmt.Fprintf(w, "HOST: %s\n", r.Host)
}
