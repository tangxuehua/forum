using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子的回复统计信息已更改的领域事件
    /// </summary>
    public class PostReplyStatisticInfoChangedEvent : DomainEvent<string>
    {
        public PostReplyStatisticInfo StatisticInfo { get; private set; }

        private PostReplyStatisticInfoChangedEvent() { }
        public PostReplyStatisticInfoChangedEvent(PostReplyStatisticInfo statisticInfo)
        {
            StatisticInfo = statisticInfo;
        }
    }
}
