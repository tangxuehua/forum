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
        public DateTime UpdatedOn { get; private set; }

        public Post() { }
        public Post(PostInfo info)
            : base(Guid.NewGuid())
        {
            if (info.ParentId == null)
            {
                RaiseEvent(new PostCreated(Id, info.ParentId, Id, info.Subject, info.Body, info.SectionId, info.AuthorId, DateTime.Now));
            }
            else
            {
                Assert.IsNotNull("info.RootId", info.RootId);
                RaiseEvent(new PostCreated(Id, info.ParentId, info.RootId.Value, info.Subject, info.Body, info.SectionId, info.AuthorId, DateTime.Now));
            }
        }

        public void ChangeBody(string body)
        {
            if (Body != body)
            {
                RaiseEvent(new PostBodyChanged(Id, ParentId, RootId, body, DateTime.Now));
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
            UpdatedOn = evnt.CreatedOn;
        }
        void IEventHandler<PostBodyChanged>.Handle(PostBodyChanged evnt)
        {
            Body = evnt.Body;
            UpdatedOn = evnt.UpdatedOn;
        }
    }
}
