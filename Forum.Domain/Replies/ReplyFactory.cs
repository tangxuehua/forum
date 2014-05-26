using ECommon.Components;
using ENode.Domain;

namespace Forum.Domain.Replies
{
    [Component(LifeStyle.Singleton)]
    public class ReplyFactory
    {
        private IRepository _repository;
        private IReplyRepository _replyRepository;

        public ReplyFactory(IReplyRepository replyRepository, IRepository repository)
        {
            _replyRepository = replyRepository;
            _repository = repository;
        }

        public Reply CreateReply(string postId, string parentId, string authorId, string body)
        {
            Reply parent = null;
            if (!string.IsNullOrEmpty(parentId))
            {
                parent = _repository.Get<Reply>(parentId);
            }
            return new Reply(_replyRepository.GetNextReplyId(), postId, parent, authorId, body);
        }
    }
}
