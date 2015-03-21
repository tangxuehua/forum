using System;
using ENode.Eventing;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子已修改的领域事件
    /// </summary>
    [Serializable]
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
