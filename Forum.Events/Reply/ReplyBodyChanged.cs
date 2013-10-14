using System;
using ENode.Eventing;

namespace Forum.Events.Reply
{
    [Serializable]
    public class ReplyBodyChanged : Event
    {
        public Guid ReplyId { get; private set; }
        public string Body { get; private set; }

        public ReplyBodyChanged(Guid replyId, string body) : base(replyId)
        {
            ReplyId = replyId;
            Body = body;
        }
    }
}
