using System.Threading.Tasks;
using System.Web.Mvc;
using ENode.Commanding;
using Forum.Commands.Replies;
using Forum.Web.Attributes;
using Forum.Web.Models;
using Forum.Web.Services;

namespace Forum.Web.Controllers
{
    public class ReplyController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IContextService _contextService;

        public ReplyController(ICommandService commandService, IContextService contextService)
        {
            _commandService = commandService;
            _contextService = contextService;
        }

        [HttpPost]
        [AjaxAuthorize]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Create(CreateReplyModel model)
        {
            var result = await _commandService.SendAsync(
                new CreateReplyCommand(
                    model.PostId,
                    model.ParentId,
                    model.Body,
                    _contextService.CurrentAccount.AccountId));

            if (result.Status == CommandSendStatus.Failed)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
    }
}