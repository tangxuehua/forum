using System.Threading.Tasks;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Replies;
using Forum.QueryServices;
using Forum.Web.Models;
using Forum.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class ReplyController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IContextService _contextService;
        private readonly IReplyQueryService _queryService;

        public ReplyController(ICommandService commandService, IContextService contextService, IReplyQueryService queryService)
        {
            _commandService = commandService;
            _contextService = contextService;
            _queryService = queryService;
        }

        [HttpGet]
        public ActionResult Find(string id, string option)
        {
            return Json(new
            {
                success = true,
                data = _queryService.FindDynamic(id, option)
            });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateReplyModel model)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            var result = await _commandService.ExecuteAsync(
                new CreateReplyCommand(
                    ObjectId.GenerateNewStringId(),
                    model.PostId,
                    model.ParentId,
                    model.Body,
                    currentAccount.AccountId), CommandReturnType.EventHandled);

            if (result.Status == CommandStatus.Failed)
            {
                return Json(new { success = false, errorMsg = result.Result });
            }

            return Json(new { success = true });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(EditReplyModel model)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            if (model.AuthorId != currentAccount.AccountId)
            {
                return Json(new { success = false, errorMsg = "您不是回复的作者，不能编辑该回复。" });
            }

            var result = await _commandService.ExecuteAsync(new ChangeReplyBodyCommand(model.Id, model.Body));

            if (result.Status == CommandStatus.Failed)
            {
                return Json(new { success = false, errorMsg = result.Result });
            }

            return Json(new { success = true });
        }
    }
}