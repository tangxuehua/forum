function PostController($scope, $http) {
    $scope.posts = [];

    $http({
        method: 'GET',
        url: '/api/posts',
        params: { authorId: authorId, sectionId: sectionId, pageIndex: pageIndex }
    })
    .success(function (data, status, headers, config) {
        $scope.posts = data;
    })
    .error(function (data, status, headers, config) {
        alert("Get posts failed.");
    });
}