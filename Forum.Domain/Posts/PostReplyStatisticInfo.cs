using System;

namespace Forum.Domain.Posts
{
    /// <summary>帖子的回复统计信息，值对象
    /// </summary>
    public class PostReplyStatisticInfo
    {
        public string LastReplyId { get; private set; }
        public string LastReplyAuthorId { get; private set; }
        public DateTime LastReplyTime { get; private set; }
        public int ReplyCount { get; private set; }

        public PostReplyStatisticInfo(string lastReplyId, string lastReplyAuthorId, DateTime lastReplyTime, int replyCount)
        {
            LastReplyId = lastReplyId;
            LastReplyAuthorId = lastReplyAuthorId;
            LastReplyTime = lastReplyTime;
            ReplyCount = replyCount;
        }
    }
}
