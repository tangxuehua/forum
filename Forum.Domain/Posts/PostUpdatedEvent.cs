using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子已修改的领域事件
    /// </summary>
    [Code(12)]
    public class PostUpdatedEvent : DomainEvent<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }

        private PostUpdatedEvent() { }
        public PostUpdatedEvent(Post post, string subject, string body) : base(post)
        {
            Subject = subject;
            Body = body;
        }
    }
}
