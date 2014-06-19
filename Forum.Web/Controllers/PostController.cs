using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ENode.Commanding;
using Forum.Commands.Posts;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;
using Forum.Web.Attributes;
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

        public ActionResult Index(string sectionId, string authorId, int? pageIndex)
        {
            var posts = _queryService.Find(
                new PostQueryOption
                {
                    SectionId = sectionId,
                    AuthorId = authorId,
                    PageInfo = new PageInfo(pageIndex == null ? 1 : pageIndex.Value)
                });

            return View(posts.Select(x => x.ToListViewModel()));
        }
        public ActionResult Detail(string id)
        {
            ViewBag.CurrentAccountId = _contextService.CurrentAccount.AccountId;
            return View(_queryService.Find(id).ToDetailViewModel());
        }
        public ActionResult Find(string id, string option)
        {
            var post = _queryService.Find(id, option);
            return Json(new
            {
                success = true,
                data = new
                {
                    id = post.id,
                    subject = post.subject,
                    body = post.body,
                    authorId = post.authorId
                }
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

            if (result.Status == CommandSendStatus.Failed)
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

            if (result.Status == CommandSendStatus.Failed)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
    }
}