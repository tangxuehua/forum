using System;
using System.Web;
using System.Web.Security;
using ECommon.Components;

namespace Forum.Web.Services
{
    [Component]
    public class AuthenticationService : IAuthenticationService
    {
        public void SignIn(string accountId, string accountName, bool createPersistentCookie)
        {
            var now = DateTime.Now;

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                accountName,
                now,
                now.AddYears(10),
                createPersistentCookie,
                accountId,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie)
            {
                cookie.Expires = ticket.Expiration;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1),
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}