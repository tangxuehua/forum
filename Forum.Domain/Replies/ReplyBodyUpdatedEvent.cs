using System;
using ENode.Eventing;

namespace Forum.Domain.Replies
{
    [Serializable]
    public class ReplyBodyUpdatedEvent : DomainEvent<string>
    {
        public string Body { get; private set; }

        public ReplyBodyUpdatedEvent(string id, string body) : base(id)
        {
            Body = body;
        }
    }
}
