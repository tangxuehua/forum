using System.Collections.Generic;
using System.Web.Http;
using ENode.Commanding;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;

namespace Forum.Api.Controllers
{
    [RoutePrefix("api/posts")]
    public class PostsController : ApiController
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _postQueryService;

        public PostsController(ICommandService commandService, IPostQueryService postQueryService)
        {
            _commandService = commandService;
            _postQueryService = postQueryService;
        }

        [Route("pages/{pageIndex:int}")]
        public IEnumerable<PostInfo> GetPosts(int pageIndex)
        {
            return _postQueryService.QueryPosts(new PostQueryOption(pageIndex));
        }
        [Route("{sectionId}/pages/{pageIndex:int}")]
        public IEnumerable<PostInfo> GetPosts(string sectionId, int pageIndex)
        {
            return _postQueryService.QueryPosts(new PostQueryOption(pageIndex) { SectionId = sectionId });
        }
        [Route("{id}")]
        public PostInfo GetPost(string id)
        {
            return _postQueryService.QueryPost(id);
        }
    }
}
