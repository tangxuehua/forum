using System;
using ENode.Eventing;

namespace Forum.Events.Post
{
    [Serializable]
    public class PostUpdatedEvent : DomainEvent<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public PostUpdatedEvent(string postId, string subject, string body) : base(postId)
        {
            Subject = subject;
            Body = body;
        }
    }
}
