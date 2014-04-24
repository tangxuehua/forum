using System;
using ENode.Eventing;

namespace Forum.Events.Reply
{
    [Serializable]
    public class ReplyBodyChanged : DomainEvent<string>
    {
        public string Body { get; private set; }

        public ReplyBodyChanged(string replyId, string body)
            : base(replyId)
        {
            Body = body;
        }
    }
}
