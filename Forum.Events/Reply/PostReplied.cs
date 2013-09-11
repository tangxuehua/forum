using System;
using ENode.Eventing;

namespace Forum.Events.Reply
{
    /// <summary>表示帖子被回复的领域事件
    /// </summary>
    [Serializable]
    public class PostReplied : Event
    {
        /// <summary>回复事件的信息
        /// </summary>
        public RepliedEventInfo Info { get; private set; }

        public PostReplied(RepliedEventInfo info)
        {
            Info = info;
        }
    }
}