HomeController=["$scope","$http", ($scope,$http) ->
  $http.get("/api/Feed").success (data) ->
    $scope.feeds = data

  #点击订阅,显示RSS条目.
  $scope.showContent=(id,type)->
    $http.get("/api/rss?title=#{id}").success (data) ->
      $scope.rss = data.Table
      debugger
]