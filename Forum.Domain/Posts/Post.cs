using System;
using ENode.Domain;
using Forum.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>帖子聚合根
    /// </summary>
    [Serializable]
    public class Post : AggregateRoot<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string SectionId { get; private set; }
        public string AuthorId { get; private set; }

        public Post(string id, string subject, string body, string sectionId, string authorId)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);
            Assert.IsNotNullOrWhiteSpace("帖子所属版块", sectionId);
            Assert.IsNotNullOrWhiteSpace("帖子作者", authorId);
            if (subject.Length > 256)
            {
                throw new Exception("帖子标题长度不能超过256");
            }
            if (body.Length > 1000)
            {
                throw new Exception("帖子内容长度不能超过1000");
            }
            ApplyEvent(new PostCreatedEvent(Id, subject, body, sectionId, authorId));
        }

        public void Update(string subject, string body)
        {
            Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);
            if (subject.Length > 256)
            {
                throw new Exception("帖子标题长度不能超过256");
            }
            if (body.Length > 1000)
            {
                throw new Exception("帖子内容长度不能超过1000");
            }
            ApplyEvent(new PostUpdatedEvent(Id, subject, body));
        }

        private void Handle(PostCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            Subject = evnt.Subject;
            Body = evnt.Body;
            SectionId = evnt.SectionId;
            AuthorId = evnt.AuthorId;
        }
        private void Handle(PostUpdatedEvent evnt)
        {
            Subject = evnt.Subject;
            Body = evnt.Body;
        }
    }
}
