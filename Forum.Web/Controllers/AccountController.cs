using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;
using Forum.QueryServices;
using Forum.Web.Attributes;
using Forum.Web.Models;
using Forum.Web.Services;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICommandService _commandService;
        private readonly IAccountQueryService _queryService;

        public AccountController(IAuthenticationService authenticationService, ICommandService commandService, IAccountQueryService queryService)
        {
            _authenticationService = authenticationService;
            _commandService = commandService;
            _queryService = queryService;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Register(RegisterModel model, CancellationToken token)
        {
            var result = await _commandService.Execute(new CreateAccountCommand(model.AccountName, model.Password));

            if (result.Status == CommandStatus.Failed)
            {
                if (result.ExceptionTypeName == typeof(DuplicateAccountNameException).Name)
                {
                    return Json(new { success = false, errorMsg = "该用户已被注册，请用其他账号注册。" });
                }
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            _authenticationService.SignIn(result.AggregateRootId, model.AccountName, false);
            return Json(new { success = true });
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var account = await Task.Factory.StartNew(() => _queryService.Find(model.AccountName));

            if (account == null)
            {
                return Json(new { success = false, errorMsg = "用户名不存在" });
            }
            else if (account.Password != model.Password)
            {
                return Json(new { success = false, errorMsg = "密码输入错误" });
            }

            _authenticationService.SignIn(account.Id, model.AccountName, model.RememberMe);

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LogOff()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
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