/*****************************************************************************
 *
 * @author 郑志强
 * 
 * @requires jQuery
 * 
 * @description  
 *
 *****************************************************************/
var com = (function ($, window) {

    var com = function ($, window) { };
    /**
     * 对象的基本属性
     * 
     */
    com.fn = com.prototype = {
        topJQuery: top.$,
        topWindow: top.window,
        parentJQuery: parent ? null : parent.$,
        parentWindow: parent ? null : parent.window
    };


    return new com($, window);

})($, window);

com = (function (com, $) {
    $.extend($.prototype, {
        htmlCode: function () {
            var $temp = $("<div/>");
            $temp.append(this);
            return $temp.html();
        }
    });

    com.isStringEmpty = function (input) {
        if (input == null || input === "" || input.trim() === "") {
            return true;
        }
        return false;
    };

    return com;


})(com, $);

com = (function (com, $) {
    /****
    * 表单提交
    */
    com.submitForm = function (options) {
        var opts = $.extend(true, {
            url: "",
            $form: $("form:first"),
            success: function (result) { },//自定义成功
            error: function (result) { alert("发现系统错误 <BR/>错误码：" + result.status + "<BR/>"); },
            isValid: false,
            headers: {}
        }, options);
        var url = opts.url || opts.$form.attr("action");
        if (!url) { alert("未设置表单提交地址!"); return; }

        if (!opts.isValid || opts.$form.valid()) {
            var ajaxopts = {
                url: url,
                dataType: "json",
                method: "POST",
                error: opts.error,
                beforeSubmit: function (formData, jqForm) {
                    //针对复选框和单选框 处理
                    $(":checkbox,:radio", jqForm).each(function () {
                        if (!existInFormData(formData, this.name)) {
                            formData.push({ name: this.name, type: this.type, value: this.checked });
                        }
                    });
                    try {
                        for (var i = 0, l = formData.length; i < l; i++) {
                            var o = formData[i];
                            if (o.type === "checkbox" || o.type === "radio") {
                                if (o.value === "on") {
                                    o.value = $("input[name='" + o.name + "']", jqForm)[0].checked ? "true" : "false";
                                }
                            }
                        }
                    } catch (e) { alert(e.message); }
                },
                beforeSend: function (a, b, c) { },
                complete: function () { },
                success: opts.success,
                headers: opts.headers
            };
            opts.$form.ajaxSubmit(ajaxopts);
        }

        function existInFormData(formData, name) {
            for (var i = 0, l = formData.length; i < l; i++) {
                var o = formData[i];
                if (o.name === name) return true;
            }
            return false;
        }
    }

    return com;

})(com, $);

