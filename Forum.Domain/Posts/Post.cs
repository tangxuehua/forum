using System;
using ENode.Domain;
using Forum.Events.Post;

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
        public DateTime CreatedOn { get; private set; }

        public Post(string id, string subject, string body, string sectionId, string authorId) : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);
            RaiseEvent(new PostCreated(Id, subject, body, sectionId, authorId, DateTime.Now));
        }

        public void ChangeSubjectAndBody(string subject, string body)
        {
            if (Subject == subject || Body == body) return;

            Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);
            RaiseEvent(new PostSubjectAndBodyChanged(Id, subject, body));
        }

        private void Handle(PostCreated evnt)
        {
            Id = evnt.AggregateRootId;
            Subject = evnt.Subject;
            Body = evnt.Body;
            SectionId = evnt.SectionId;
            AuthorId = evnt.AuthorId;
            CreatedOn = evnt.CreatedOn;
        }
        private void Handle(PostSubjectAndBodyChanged evnt)
        {
            Subject = evnt.Subject;
            Body = evnt.Body;
        }
    }
}
