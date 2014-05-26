using ECommon.Components;
using ECommon.Utilities;
using Forum.Domain.Replies;

namespace Forum.Domain.Repositories.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class ReplyRepository : IReplyRepository
    {
        public string GetNextReplyId()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}
