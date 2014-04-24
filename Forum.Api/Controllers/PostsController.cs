using System.Collections.Generic;
using System.Web.Http;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;

namespace Forum.Api.Controllers
{
    public class PostsController : ApiController
    {
        private readonly IPostQueryService _postQueryService;

        public PostsController(IPostQueryService postQueryService)
        {
            _postQueryService = postQueryService;
        }

        public IEnumerable<Post> GetPosts(int pageIndex)
        {
            return _postQueryService.QueryPosts(new PostQueryOption(pageIndex));
        }
        public IEnumerable<Post> GetPosts(string sectionId, int pageIndex)
        {
            return _postQueryService.QueryPosts(new PostQueryOption(pageIndex) { SectionId = sectionId });
        }
        public PostInfo GetPost(string id)
        {
            return _postQueryService.QueryPost(id);
        }
    }
}
