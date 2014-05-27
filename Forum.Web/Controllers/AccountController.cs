using System;
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
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var command = new RegisterNewAccountCommand(model.AccountName, model.Password);
            var task = _commandService.Execute(command);
            var result = task.WaitResult<CommandResult>(5000);

            if (!task.IsCompleted)
            {
                ModelState.AddModelError("", "用户注册处理超时。");
            }
            else if (result.Status == CommandStatus.Success)
            {
                FormsAuthentication.SetAuthCookie(model.AccountName, false);
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
        public Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            var viewModelTask = Task.Factory.StartNew(() => model);

            if (!ModelState.IsValid)
            {
                return viewModelTask.ContinueWith<ActionResult>(task => View(task.Result));
            }

            var queryAccountTask = Task.Factory.StartNew(() => _accountQueryService.Find(model.AccountName));

            return Task.Factory.ContinueWhenAll<ActionResult>(new Task[] { viewModelTask, queryAccountTask },
                tasks =>
                {
                    var viewModel = ((Task<LoginModel>)tasks[0]).Result;
                    var account = ((Task<AccountData>)tasks[1]).Result;

                    if (account == null)
                    {
                        ModelState.AddModelError("", "账号不存在。");
                        return View(viewModel);
                    }
                    else if (account.Password != model.Password)
                    {
                        ModelState.AddModelError("", "密码不正确。");
                        return View(viewModel);
                    }
                    else
                    {
                        SignIn(viewModel.AccountName, viewModel.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                });
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