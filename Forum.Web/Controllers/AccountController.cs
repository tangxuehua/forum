using System.Web.Mvc;
using System.Web.Security;
using ECommon.Extensions;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;
using Forum.QueryServices;
using Forum.Web.Models;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IAccountService _accountService;

        public AccountController(ICommandService commandService, IAccountService accountService)
        {
            _commandService = commandService;
            _accountService = accountService;
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

            var command = new CreateAccountCommand(ObjectId.GenerateNewStringId(), model.UserName, model.Password);
            var task = _commandService.Execute(command);
            var result = task.WaitResult<CommandResult>(5000);

            if (!task.IsCompleted)
            {
                ModelState.AddModelError("", "用户注册处理超时。");
            }
            else if (result.Status == CommandStatus.Success)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
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
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var account = _accountService.GetAccount(model.UserName);
            if (account == null)
            {
                ModelState.AddModelError("", "账号不存在。");
            }
            else if (account.Password != model.Password)
            {
                ModelState.AddModelError("", "密码不正确。");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

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