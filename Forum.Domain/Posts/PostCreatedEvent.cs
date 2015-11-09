using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>表示帖子已创建的领域事件
    /// </summary>
    [Code(11)]
    public class PostCreatedEvent : DomainEvent<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string SectionId { get; private set; }
        public string AuthorId { get; private set; }

        private PostCreatedEvent() { }
        public PostCreatedEvent(Post post, string subject, string body, string sectionId, string authorId) : base(post)
        {
            Subject = subject;
            Body = body;
            SectionId = sectionId;
            AuthorId = authorId;
        }
    }
}
