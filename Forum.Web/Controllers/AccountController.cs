using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Infrastructure;
using Forum.QueryServices;
using Forum.Web.Extensions;
using Forum.Web.Models;
using Forum.Web.Services;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICommandService _commandService;
        private readonly IAccountQueryService _queryService;

        public AccountController(IAuthenticationService authenticationService
            , ICommandService commandService
            , IAccountQueryService queryService)
        {
            _authenticationService = authenticationService;
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Register(RegisterModel model, CancellationToken token)
        {
            string pwd = PasswordHash.PasswordHash.CreateHash(model.Password);
            var result = await _commandService.ExecuteAsync(new RegisterNewAccountCommand(ObjectId.GenerateNewStringId(), model.AccountName, pwd), CommandReturnType.EventHandled);
            if (result.Status != AsyncTaskStatus.Success)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            var commandResult = result.Data;
            if (commandResult.Status == CommandStatus.Failed)
            {
                if (commandResult.ResultType == typeof(DuplicateAccountException).Name)
                {
                    return Json(new { success = false, errorMsg = "该账号已被注册，请用其他账号注册。" });
                }
                return Json(new { success = false, errorMsg = commandResult.Result });
            }

            _authenticationService.SignIn(commandResult.AggregateRootId, model.AccountName, false);
            return Json(new { success = true });
        }
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginModel());
        }
        [HttpPost]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var account = await Task.Factory.StartNew(() => _queryService.Find(model.AccountName));

            if (account == null)
            {
                return Json(new { success = false, errorMsg = "账号不存在。" });
            }
            else if (!PasswordHash.PasswordHash.ValidatePassword(model.Password, account.Password))
            {
                return Json(new { success = false, errorMsg = "密码不正确。" });
            }

            _authenticationService.SignIn(account.Id, model.AccountName, model.RememberMe);

            return Json(new { success = true });
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LogOff()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}