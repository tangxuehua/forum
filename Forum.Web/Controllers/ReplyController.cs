﻿using System.Threading.Tasks;
using System.Web.Mvc;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Replies;
using Forum.QueryServices;
using Forum.Web.Extensions;
using Forum.Web.Models;
using Forum.Web.Services;

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
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAuthorize]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(CreateReplyModel model)
        {
            var result = await _commandService.ExecuteAsync(
                new CreateReplyCommand(
                    ObjectId.GenerateNewStringId(),
                    model.PostId,
                    model.ParentId,
                    model.Body,
                    _contextService.CurrentAccount.AccountId), CommandReturnType.EventHandled);

            if (result.Status != AsyncTaskStatus.Success)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
        [HttpPost]
        [AjaxAuthorize]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        [ValidateInput(false)]
        public async Task<ActionResult> Update(EditReplyModel model)
        {
            if (model.AuthorId != _contextService.CurrentAccount.AccountId)
            {
                return Json(new { success = false, errorMsg = "您不是回复的作者，不能编辑该回复。" });
            }

            var result = await _commandService.SendAsync(new ChangeReplyBodyCommand(model.Id, model.Body));

            if (result.Status != AsyncTaskStatus.Success)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
    }
}