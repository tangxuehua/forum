using System.Collections.Generic;
using System.Web.Http;
using ENode.Commanding;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;

namespace Forum.Web.Controllers
{
    public class PostsController : ApiController
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _postQueryService;

        public PostsController(ICommandService commandService, IPostQueryService postQueryService)
        {
            _commandService = commandService;
            _postQueryService = postQueryService;
        }

        [Route("api/posts")]
        public IEnumerable<PostInfo> GetPosts(string sectionId, string authorId, int pageIndex)
        {
            return _postQueryService.Find(new PostQueryOption { SectionId = sectionId, AuthorId = authorId, PageInfo = new PageInfo(pageIndex) });
        }
        [Route("api/posts/{id}")]
        public PostInfo GetPost(string id)
        {
            return _postQueryService.Find(id);
        }
    }
}
