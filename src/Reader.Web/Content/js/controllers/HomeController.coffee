HomeController=["$scope","$http", ($scope,$http) ->
  $http.get("/api/Feed").success (data) ->
    $scope.feeds = data

  #点击订阅,显示RSS条目.
  $scope.showContent=(rss,type)->
    $scope.currentFeed=rss
    $http.get("/api/rss?title=#{rss.Title}").success (data) ->
      $scope.rss = data.Table
      debugger
]