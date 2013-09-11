using System;

namespace Forum.Events.Reply
{
    /// <summary>值对象，描述一个回复事件的信息
    /// </summary>
    [Serializable]
    public class RepliedEventInfo
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

        public RepliedEventInfo(Guid replyId, Guid postId, string body, Guid authorId, DateTime createdOn)
        {
            ReplyId = replyId;
            PostId = postId;
            Body = body;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}