using System;
using ENode.Eventing;

namespace Forum.Events.Post
{
    /// <summary>表示帖子已被发表的领域事件
    /// </summary>
    [Serializable]
    public class PostCreated : DomainEvent<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string SectionId { get; private set; }
        public string AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public PostCreated(string postId, string subject, string body, string sectionId, string authorId, DateTime createdOn)
            : base(postId)
        {
            Subject = subject;
            Body = body;
            SectionId = sectionId;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}
