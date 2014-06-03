function PostDetailController($scope, $http) {
    $scope.showNewReplyDialog = function () {
        $scope.newReply = {
            body: '',
            postId: postId,
            parentId: ''
        };
        $("#float-box-newReply").modal("show");
    };

    $scope.submitReply = function () {
        if (isStringEmpty($scope.newReply.body)) {
            msg('请输入回复内容');
            return false;
        }
        if (isStringEmpty($scope.newReply.postId)) {
            msg('回复对应的帖子不能为空');
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
                msg(result.errorMsg);
            }
        })
        .error(function (result, status, headers, config) {
            msg(result.errorMsg);
        });
    };
}