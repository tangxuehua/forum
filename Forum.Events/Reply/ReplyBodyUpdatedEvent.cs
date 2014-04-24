using System;
using ENode.Eventing;

namespace Forum.Events.Reply
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
