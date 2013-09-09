using System;
using ENode.Domain;
using ENode.Eventing;
using Forum.Domain.Posts.Events;

namespace Forum.Domain.Posts.Model
{
    /// <summary>帖子聚合根
    /// </summary>
    [Serializable]
    public class Post : AggregateRoot<Guid>,
        IEventHandler<PostCreated>,
        IEventHandler<PostSubjectAndBodyChanged>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public Guid SectionId { get; private set; }
        public Guid AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Post() { }
        public Post(string subject, string body, Guid sectionId, Guid authorId) : base(Guid.NewGuid())
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

        void IEventHandler<PostCreated>.Handle(PostCreated evnt)
        {
            Subject = evnt.Subject;
            Body = evnt.Body;
            SectionId = evnt.SectionId;
            AuthorId = evnt.AuthorId;
            CreatedOn = evnt.CreatedOn;
        }
        void IEventHandler<PostSubjectAndBodyChanged>.Handle(PostSubjectAndBodyChanged evnt)
        {
            Subject = evnt.Subject;
            Body = evnt.Body;
        }
    }
}
