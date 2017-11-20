using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Infrastructure;
using Forum.QueryServices;
using Forum.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IAccountQueryService _queryService;

        public AccountController(ICommandService commandService, IAccountQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            await SignInAsync(commandResult.AggregateRootId, model.AccountName, false);
            return Json(new { success = true });
        }
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            await SignInAsync(account.Id, model.AccountName, model.RememberMe);
            return Json(new { success = true });
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private Task SignInAsync(string accountId, string accountName, bool persistentCookie)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, accountId),
                new Claim(ClaimTypes.Name, accountName)
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = persistentCookie
            };
            if (persistentCookie)
            {
                authenticationProperties.ExpiresUtc = DateTime.UtcNow.AddDays(365);
            }
            else
            {
                authenticationProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);
            }

            return HttpContext.SignInAsync(user, authenticationProperties);
        }
    }
}