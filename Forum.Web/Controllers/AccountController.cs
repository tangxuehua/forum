using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Forum.Web.Models;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            //try
            //{
            //    var result = _commandService.Execute(new CreateAccount { Name = model.UserName, Password = model.Password, MillisecondsTimeout = 10000 });
            //    if (result.IsCompleted && result.ErrorInfo == null)
            //    {
            //        FormsAuthentication.SetAuthCookie(model.UserName, false);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    if (result.ErrorInfo != null)
            //    {
            //        ModelState.AddModelError("", result.ErrorInfo.GetErrorMessage());
            //    }
            //    else if (!result.IsCompleted)
            //    {
            //        ModelState.AddModelError("", "用户注册处理超时。");
            //    }
            //}
            //catch (CommandExecutionException ex)
            //{
            //    if (ex.InnerException is DuplicateAccountNameException)
            //    {
            //        ModelState.AddModelError("", "该用户已被注册，请用其他账号注册。");
            //    }
            //}
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
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            //var account = _accountService.GetAccount(model.UserName);
            //if (account == null)
            //{
            //    ModelState.AddModelError("", "账号不存在。");
            //}
            //else if (account.Password != model.Password)
            //{
            //    ModelState.AddModelError("", "密码不正确。");
            //}
            //else
            //{
            //    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            //    return RedirectToLocal(returnUrl);
            //}

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
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
    }
}