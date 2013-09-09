using System;
using ENode.Eventing;

namespace Forum.Domain.Replies.Events
{
    [Serializable]
    public class ReplyBodyChanged : Event
    {
        public Guid ReplyId { get; private set; }
        public string Body { get; private set; }

        public ReplyBodyChanged(Guid replyId, string body)
        {
            ReplyId = replyId;
            Body = body;
        }
    }
}
