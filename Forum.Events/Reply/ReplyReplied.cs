using System;
using ENode.Eventing;

namespace Forum.Events.Reply
{
    /// <summary>表示回复被回复的领域事件
    /// </summary>
    [Serializable]
    public class ReplyReplied : Event
    {
        /// <summary>被回复的回复ID
        /// </summary>
        public Guid ParentReplyId { get; private set; }
        /// <summary>回复事件的信息
        /// </summary>
        public RepliedEventInfo Info { get; private set; }

        public ReplyReplied(Guid parentReplyId, RepliedEventInfo info) : base(info.ReplyId)
        {
            ParentReplyId = parentReplyId;
            Info = info;
        }
    }
}
