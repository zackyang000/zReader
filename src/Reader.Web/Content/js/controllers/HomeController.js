var HomeController;

HomeController = [
  "$scope", "$http", function($scope, $http) {
    $http.get("/api/Feed").success(function(data) {
      return $scope.feeds = data;
    });
    return $scope.showContent = function(rss, type) {
      $scope.currentFeed = rss;
      return $http.get("/api/rss?title=" + rss.Title).success(function(data) {
        $scope.rss = data.Table;
        debugger;
      });
    };
  }
];
