using System;
using System.Net;
using System.Web.Mvc;

namespace Forum.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        errorMsg = GetErrorMessage(filterContext)
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.ExceptionHandled = true;
            }
            else
            {
                if (filterContext.Exception != null && filterContext.Exception is TimeoutException)
                {
                    View = "TimeoutError";
                }
                base.OnException(filterContext);
            }
        }

        private static string GetErrorMessage(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                if (filterContext.Exception is TimeoutException)
                {
                    return "服务器处理请求超时";
                }
                return filterContext.Exception.Message;
            }
            return "服务器未知错误";
        }
    }
}