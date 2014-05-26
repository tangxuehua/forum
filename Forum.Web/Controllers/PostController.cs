using System.Linq;
using System.Web.Mvc;
using ENode.Commanding;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;
using Forum.Web.Extensions;

namespace Forum.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _postQueryService;

        public PostController(ICommandService commandService, IPostQueryService postQueryService)
        {
            _commandService = commandService;
            _postQueryService = postQueryService;
        }

        public ActionResult Index(string sectionId, string authorId, int? pageIndex)
        {
            var posts = _postQueryService.Find(
                new PostQueryOption
                {
                    SectionId = sectionId,
                    AuthorId = authorId,
                    PageInfo = new PageInfo(pageIndex == null ? 1 : pageIndex.Value)
                });

            var model = posts.Select(x => x.ToViewModel());

            return View(model);
        }
        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}