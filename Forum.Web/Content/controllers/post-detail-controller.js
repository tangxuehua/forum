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
    $scope.showEditPostDialog = function () {
        $scope.errorMsg = '';
        $scope.editingPost = {
            subject: '',
            body: '',
            id: postId,
            authorId: postAuthorId
        };
        $("#float-box-editingPost").modal("show");
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

    $scope.updatePost = function () {
        if (isStringEmpty($scope.editingPost.subject)) {
            $scope.errorMsg = '帖子标题不能为空';
            return false;
        }
        if (isStringEmpty($scope.editingPost.body)) {
            $scope.errorMsg = '帖子内容不能为空';
            return false;
        }

        $http({
            method: 'POST',
            url: '/post/update',
            data: $scope.editingPost
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