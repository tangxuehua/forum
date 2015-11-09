using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子的回复统计信息已更改的领域事件
    /// </summary>
    [Code(13)]
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
