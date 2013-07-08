HomeController=["$scope","$http", ($scope,$http) ->
  $http.get('/api/Feed').success (data) ->
    $scope.feeds = data
]