using System;
using ENode.Eventing;

namespace Forum.Domain.Replies.Events
{
    /// <summary>表示帖子被回复的领域事件
    /// </summary>
    [Serializable]
    public class PostReplied : Event
    {
        /// <summary>回复ID
        /// </summary>
        public Guid ReplyId { get; private set; }
        /// <summary>被回复的帖子ID
        /// </summary>
        public Guid PostId { get; private set; }
        /// <summary>回复内容
        /// </summary>
        public string Body { get; private set; }
        /// <summary>回复人ID
        /// </summary>
        public Guid AuthorId { get; private set; }
        /// <summary>回复时间
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        public PostReplied(Guid replyId, Guid postId, string body, Guid authorId, DateTime createdOn)
        {
            ReplyId = replyId;
            PostId = postId;
            Body = body;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}