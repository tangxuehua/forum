using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子已修改的领域事件
    /// </summary>
    public class PostUpdatedEvent : DomainEvent<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }

        private PostUpdatedEvent() { }
        public PostUpdatedEvent(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}
