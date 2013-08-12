using System.Web.Mvc;
using System.Web.Security;
using ENode.Commanding;
using Forum.Application.Commands;
using Forum.Domain;
using Forum.Repository;
using Forum.Web.Filters;
using Forum.Web.Models;

namespace Forum.Web.Controllers {
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller {
        private ICommandService _commandService;
        private IAccountRepository _accountRepository;

        public AccountController(ICommandService commandService, IAccountRepository accountRepository) {
            _commandService = commandService;
            _accountRepository = accountRepository;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl) {

            if (ModelState.IsValid) {
                var account = _accountRepository.GetAccount(model.UserName);
                if (account == null) {
                    ModelState.AddModelError("", "账号不存在。");
                }
                else if (account.Password != model.Password) {
                    ModelState.AddModelError("", "密码不正确。");
                }
                else {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model) {
            if (ModelState.IsValid) {
                try {
                    var result = _commandService.Execute(new CreateAccount { Name = model.UserName, Password = model.Password, MillisecondsTimeout = 100000 });
                    if (result.IsCompleted && !result.HasError) {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.HasError) {
                        ModelState.AddModelError("", result.ErrorMessage);
                    }
                    else if (!result.IsCompleted) {
                        ModelState.AddModelError("", "用户注册处理超时。");
                    }
                }
                catch (CommandExecuteException ex) {
                    if (ex.InnerException != null && ex.InnerException is DuplicateAccountNameException) {
                        ModelState.AddModelError("", "该用户已被注册，请用其他账号注册。");
                    }
                }
            }
            return View(model);
        }

        public ActionResult Manage() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model) {
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
