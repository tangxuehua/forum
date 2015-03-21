using System;
using ENode.Eventing;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子的回复统计信息已更改的领域事件
    /// </summary>
    [Serializable]
    public class PostReplyStatisticInfoChangedEvent : DomainEvent<string>
    {
        public PostReplyStatisticInfo StatisticInfo { get; private set; }

        private PostReplyStatisticInfoChangedEvent() { }
        public PostReplyStatisticInfoChangedEvent(Post post, PostReplyStatisticInfo statisticInfo)
            : base(post)
        {
            StatisticInfo = statisticInfo;
        }
    }
}
