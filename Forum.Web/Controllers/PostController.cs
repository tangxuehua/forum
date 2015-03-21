using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Posts;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;
using Forum.Web.Controls;
using Forum.Web.Extensions;
using Forum.Web.Models;
using Forum.Web.Services;

namespace Forum.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _queryService;
        private readonly IContextService _contextService;

        public PostController(ICommandService commandService, IPostQueryService queryService, IContextService contextService)
        {
            _commandService = commandService;
            _queryService = queryService;
            _contextService = contextService;
        }

        [HttpGet]
        public ActionResult Index(string sectionId, string authorId, int? page)
        {
            var pageIndex = page == null ? 1 : page.Value;
            if (pageIndex <= 0) pageIndex = 1;
            var result = _queryService.Find(
                new PostQueryOption
                {
                    SectionId = sectionId,
                    AuthorId = authorId,
                    PageInfo = new PageInfo(pageIndex)
                });
            ViewBag.SectionId = sectionId;
            ViewBag.AuthorId = authorId;
            ViewBag.Pager = Pager.Items(result.TotalCount).PerPage(20).Move(pageIndex).Segment(5).Center();
            return View(result.Posts.Select(x => x.ToListViewModel()));
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            ViewBag.CurrentAccountId = _contextService.CurrentAccount != null ? _contextService.CurrentAccount.AccountId : null;
            return View(_queryService.Find(id).ToDetailViewModel());
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
        public async Task<ActionResult> Create(CreatePostModel model)
        {
            var result = await _commandService.SendAsync(
                new CreatePostCommand(
                    model.Subject,
                    model.Body,
                    model.SectionId,
                    _contextService.CurrentAccount.AccountId));

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
        public async Task<ActionResult> Update(EditPostModel model)
        {
            if (model.AuthorId != _contextService.CurrentAccount.AccountId)
            {
                return Json(new { success = false, errorMsg = "您不是帖子的作者，不能编辑该帖子。" });
            }

            var result = await _commandService.SendAsync(new UpdatePostCommand(model.Id, model.Subject, model.Body));

            if (result.Status != AsyncTaskStatus.Success)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
    }
}