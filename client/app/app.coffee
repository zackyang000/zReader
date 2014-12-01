angular.module("app",
[
  'ngRoute'
  'ngCookies'
  'ngSanitize'
])

.controller('HomeController', [ '$scope', '$http', ($scope, $http) ->

  $scope.feeds = $http.get("#{config.url.api}/feeds?username=zackyang")

  #点击订阅,显示RSS条目.
  $scope.showContent=(feed, type)->
    $scope.currentFeed = feed
    $scope.rss = $http.get("/api/rss?title=#{feed.Title}")
])
