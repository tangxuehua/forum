using System;
using ENode.Eventing;

namespace Forum.Events.Reply
{
    /// <summary>表示回复被回复的领域事件
    /// </summary>
    [Serializable]
    public class ReplyReplied : Event
    {
        /// <summary>回复ID
        /// </summary>
        public Guid ReplyId { get; private set; }
        /// <summary>被回复的回复ID
        /// </summary>
        public Guid ParentReplyId { get; private set; }
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

        public ReplyReplied(Guid replyId, Guid parentReplyId, Guid postId, string body, Guid authorId, DateTime createdOn)
        {
            ReplyId = replyId;
            ParentReplyId = parentReplyId;
            PostId = postId;
            Body = body;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}
