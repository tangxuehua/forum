using ECommon.Components;
using Forum.Domain.Posts;

namespace Forum.Domain.Posts
{
    [Component(LifeStyle.Singleton)]
    public class PostFactory
    {
        private IPostRepository _repository;

        public PostFactory(IPostRepository repository)
        {
            _repository = repository;
        }

        public Post CreatePost(string subject, string body, string sectionId, string authorId)
        {
            return new Post(_repository.GetNextPostId(), subject, body, sectionId, authorId);
        }
    }
}
