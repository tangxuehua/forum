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
        public string ReplyId { get; private set; }
        /// <summary>被回复的帖子ID
        /// </summary>
        public string PostId { get; private set; }
        /// <summary>回复内容
        /// </summary>
        public string Body { get; private set; }
        /// <summary>回复人ID
        /// </summary>
        public string AuthorId { get; private set; }
        /// <summary>回复时间
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        public RepliedEventInfo(string replyId, string postId, string body, string authorId, DateTime createdOn)
        {
            ReplyId = replyId;
            PostId = postId;
            Body = body;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}