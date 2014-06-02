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
using Forum.Web.Models;
using Forum.Web.Services;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ICommandService _commandService;
        private readonly IAccountQueryService _queryService;

        public AccountController(AuthenticationService authenticationService, ICommandService commandService, IAccountQueryService queryService)
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
        [ValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Register(RegisterModel model, CancellationToken token)
        {
            var result = await _commandService.Execute(new RegisterNewAccountCommand(model.AccountName, model.Password));

            if (result.Status == CommandStatus.Success)
            {
                _authenticationService.SignIn(result.AggregateRootId, model.AccountName, false);
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

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            var account = await Task.Factory.StartNew(() => _queryService.Find(model.AccountName));

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

            _authenticationService.SignIn(account.Id, model.AccountName, model.RememberMe);

            return RedirectToLocal(returnUrl);
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