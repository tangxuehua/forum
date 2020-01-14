using System.Linq;
using System.Threading.Tasks;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Posts;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;
using Forum.Web.Controls;
using Forum.Web.Extensions;
using Forum.Web.Models;
using Forum.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _postQueryService;
        private readonly IContextService _contextService;
        private readonly ISectionQueryService _sectionQueryService;

        public PostController(ICommandService commandService, IPostQueryService queryService, IContextService contextService, ISectionQueryService sectionqueryService)
        {
            _commandService = commandService;
            _postQueryService = queryService;
            _contextService = contextService;
            _sectionQueryService = sectionqueryService;
        }

        [HttpGet]
        public ActionResult Index(string sectionId, string authorId, int? page)
        {
            var pageIndex = page == null ? 1 : page.Value;
            if (pageIndex <= 0) pageIndex = 1;
            var result = _postQueryService.Find(
                new PostQueryOption
                {
                    SectionId = sectionId,
                    AuthorId = authorId,
                    PageInfo = new PageInfo(pageIndex)
                });

            ViewBag.Section = _sectionQueryService.FindInculdeStatisticById(sectionId).ToViewModel(sectionId);
            ViewBag.SectionId = sectionId;
            ViewBag.AuthorId = authorId;
            ViewBag.Pager = Pager.Items(result.TotalCount).PerPage(20).Move(pageIndex).Segment(5).Center();
            return View(result.Posts.Select(x => x.ToListViewModel()));
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            ViewBag.CurrentAccountId = currentAccount?.AccountId;
            return View(_postQueryService.Find(id).ToDetailViewModel());
        }
        [HttpGet]
        public ActionResult Find(string id, string option)
        {
            return Json(new
            {
                success = true,
                data = _postQueryService.FindDynamic(id, option)
            });
        }
        [HttpGet]
        public ActionResult Create(string sectionId)
        {
            var section = _sectionQueryService.FindInculdeStatisticById(sectionId).ToViewModel(sectionId);
            return View(new PostDetailModel()
            {
                SectionId = section.Id,
                SectionName = section.Name
            });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePostModel model)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            var result = await _commandService.ExecuteAsync(
                new CreatePostCommand(
                    ObjectId.GenerateNewStringId(),
                    model.Subject,
                    model.Body,
                    model.SectionId,
                    currentAccount.AccountId));

            if (result.Status == CommandStatus.Failed)
            {
                return Json(new { success = false, errorMsg = result.Result });
            }

            return Json(new { success = true });
        }
        [HttpGet]
        public ActionResult Update(string id)
        {
            var model = _postQueryService.Find(id).ToDetailViewModel();
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(EditPostModel model)
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            if (model.AuthorId != currentAccount.AccountId)
            {
                return Json(new { success = false, errorMsg = "您不是帖子的作者，不能编辑该帖子。" });
            }

            var result = await _commandService.ExecuteAsync(new UpdatePostCommand(model.Id, model.Subject, model.Body));

            if (result.Status == CommandStatus.Failed)
            {
                return Json(new { success = false, errorMsg = result.Result });
            }

            return Json(new { success = true });
        }
    }
}