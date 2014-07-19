//消息弹出框
var msg = function (message) {
    bootbox.alert(message);
};
//弹出提示消息框，支持回调函数
var showMessage = function (message, callback) {
    bootbox.alert(message, function (result) {
        callback();
    });
};
//确认对话框
var showConfirm = function (message, callback) {
    bootbox.confirm(message, function (result) {
        if (result) {
            callback();
        }
    });
};

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

var isStringEmpty = function (input) {
    if (input == null || input == '' || input.trim() == '') {
        return true;
    }
    return false;
};

var app = angular.module('forum', []);
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    $httpProvider.defaults.headers.common["RequestVerificationToken"] = requestVerificationToken;
}]);