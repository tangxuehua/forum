using System;
using ENode.Domain;
using ENode.Eventing;
using Forum.Domain.Events;

namespace Forum.Domain.Model
{
    /// <summary>论坛核心模型，表示论坛中的一个帖子或回复
    /// </summary>
    [Serializable]
    public class Post : AggregateRoot<Guid>,
        IEventHandler<PostCreated>,
        IEventHandler<PostBodyChanged>
    {
        public Guid? ParentId { get; private set; }
        public Guid RootId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public Guid SectionId { get; private set; }
        public Guid AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Post() { }
        public Post(string subject, string body, Post parent, Post root, Guid sectionId, Guid authorId) : base(Guid.NewGuid())
        {
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);

            if (parent != null || root != null)
            {
                Assert.IsNotNull("回复帖子的父帖子", parent);
                Assert.IsNotNull("回复帖子的根帖子", root);
                Assert.AreEqual(parent.RootId, root.Id, "父帖子的根帖子ID {0} 与根帖子ID {1} 不同");
                RaiseEvent(new PostCreated(Id, parent.Id, root.Id, subject, body, sectionId, authorId, DateTime.Now));
            }
            else
            {
                Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
                RaiseEvent(new PostCreated(Id, null, Id, subject, body, sectionId, authorId, DateTime.Now));
            }
        }

        public void ChangeBody(string body)
        {
            if (Body != body)
            {
                RaiseEvent(new PostBodyChanged(Id, ParentId, RootId, body));
            }
        }

        void IEventHandler<PostCreated>.Handle(PostCreated evnt)
        {
            ParentId = evnt.ParentId;
            RootId = evnt.RootId;
            Subject = evnt.Subject;
            Body = evnt.Body;
            SectionId = evnt.SectionId;
            AuthorId = evnt.AuthorId;
            CreatedOn = evnt.CreatedOn;
        }
        void IEventHandler<PostBodyChanged>.Handle(PostBodyChanged evnt)
        {
            Body = evnt.Body;
        }
    }
}
