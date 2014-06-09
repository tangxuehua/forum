function PostDetailController($scope, $http) {
    $scope.errorMsg = '';

    $scope.showNewReplyDialog = function () {
        $scope.errorMsg = '';
        $scope.newReply = {
            body: '',
            postId: postId,
            parentId: ''
        };
        $("#float-box-newReply").modal("show");
    };
    $scope.showNewSubReplyDialog = function (parentId) {
        $scope.errorMsg = '';
        $scope.newReply = {
            body: '',
            postId: postId,
            parentId: parentId
        };
        $("#float-box-newReply").modal("show");
    };

    $scope.submitReply = function () {
        if (isStringEmpty($scope.newReply.body)) {
            $scope.errorMsg = '请输入回复内容';
            return false;
        }
        if (isStringEmpty($scope.newReply.postId)) {
            $scope.errorMsg = '回复对应的帖子不能为空';
            return false;
        }

        $http({
            method: 'POST',
            url: '/reply/create',
            data: $scope.newReply
        })
        .success(function (result, status, headers, config) {
            if (result.success) {
                window.location.reload();
            } else {
                $scope.errorMsg = result.errorMsg;
            }
        })
        .error(function (result, status, headers, config) {
            $scope.errorMsg = result.errorMsg;
        });
    };
}