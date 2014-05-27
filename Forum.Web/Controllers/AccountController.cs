using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ECommon.Extensions;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;
using Forum.QueryServices;
using Forum.Web.Models;
using AccountData = Forum.QueryServices.DTOs.AccountInfo;

namespace Forum.Web.Controllers
{
    public class AccountController : AsyncController
    {
        private readonly ICommandService _commandService;
        private readonly IAccountQueryService _accountQueryService;

        public AccountController(ICommandService commandService, IAccountQueryService accountQueryService)
        {
            _commandService = commandService;
            _accountQueryService = accountQueryService;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async Task<ActionResult> Register(RegisterModel model, CancellationToken token)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _commandService.Execute(new RegisterNewAccountCommand(model.AccountName, model.Password));

            if (result.Status == CommandStatus.Success)
            {
                SignIn(model.AccountName, false);
                return RedirectToAction("Index", "Home");
            }
            else if (result.Status == CommandStatus.Failed)
            {
                if (result.ExceptionTypeName == typeof(DuplicateAccountNameException).Name)
                {
                    ModelState.AddModelError("", "该用户已被注册，请用其他账号注册。");
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var account = await Task.Factory.StartNew(() => _accountQueryService.Find(model.AccountName));

            if (account == null)
            {
                ModelState.AddModelError("", "账号不存在。");
                return View(model);
            }
            else if (account.Password != model.Password)
            {
                ModelState.AddModelError("", "密码不正确。");
                return View(model);
            }

            SignIn(model.AccountName, model.RememberMe);

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ControlPanel()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private void SignIn(string accountName, bool createPersistentCookie)
        {
            var now = DateTime.Now;

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                accountName,
                now,
                now.AddYears(10),
                createPersistentCookie,
                accountName,
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

            HttpContext.Response.Cookies.Add(cookie);
        }
        private void SignOut()
        {
            FormsAuthentication.SignOut();

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1),
            };

            HttpContext.Response.Cookies.Add(cookie);
        }
    }
}