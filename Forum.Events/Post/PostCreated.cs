using System;
using ENode.Eventing;

namespace Forum.Events.Post
{
    /// <summary>表示帖子已被发表的领域事件
    /// </summary>
    [Serializable]
    public class PostCreated : Event
    {
        public Guid PostId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public Guid SectionId { get; private set; }
        public Guid AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public PostCreated(Guid postId, string subject, string body, Guid sectionId, Guid authorId, DateTime createdOn) : base(postId)
        {
            PostId = postId;
            Subject = subject;
            Body = body;
            SectionId = sectionId;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}
