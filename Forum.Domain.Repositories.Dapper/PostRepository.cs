using ECommon.Components;
using ECommon.Utilities;
using Forum.Domain.Posts;

namespace Forum.Domain.Repositories.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class PostRepository : IPostRepository
    {
        public string GetNextPostId()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}
