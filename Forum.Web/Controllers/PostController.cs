using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENode.Commanding;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;

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

        public ActionResult Index(string sectionId, int pageIndex)
        {
            return View(_postQueryService.QueryPosts(new PostQueryOption(pageIndex) { SectionId = sectionId }));
        }
        public ActionResult Detail(string id)
        {
            return View(_postQueryService.QueryPost(id));
        }
    }
}