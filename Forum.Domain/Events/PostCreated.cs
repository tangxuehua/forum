using System;
using ENode.Eventing;
using Forum.Domain.Model;

namespace Forum.Domain.Events
{
    [Serializable]
    public class PostCreated : Event
    {
        public Guid PostId { get; private set; }
        public Guid? ParentId { get; private set; }
        public Guid RootId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public Guid SectionId { get; private set; }
        public Guid AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public PostCreated(Guid postId, Guid? parentId, Guid rootId, string subject, string body, Guid sectionId, Guid authorId, DateTime createdOn)
        {
            PostId = postId;
            ParentId = parentId;
            RootId = rootId;
            Subject = subject;
            Body = body;
            SectionId = sectionId;
            AuthorId = authorId;
            CreatedOn = createdOn;
        }
    }
}
