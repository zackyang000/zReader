var HomeController;

HomeController = [
  "$scope", "$http", function($scope, $http) {
    return $http.get('/api/Feed').success(function(data) {
      return $scope.feeds = data;
    });
  }
];
