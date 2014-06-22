using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Forum.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ValidateRequestHeader(filterContext.HttpContext.Request);
            }
            else
            {
                AntiForgery.Validate();
            }
        }

        private static void ValidateRequestHeader(HttpRequestBase request)
        {
            string cookieToken = string.Empty;
            string formToken = string.Empty;
            string tokenValue = request.Headers["RequestVerificationToken"];
            if (!string.IsNullOrEmpty(tokenValue))
            {
                string[] tokens = tokenValue.Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            AntiForgery.Validate(cookieToken, formToken);
        }
    }
}