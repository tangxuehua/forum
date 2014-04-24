using System;
using ENode.Domain;
using Forum.Events.Reply;

namespace Forum.Domain.Replies
{
    /// <summary>回复聚合根
    /// </summary>
    [Serializable]
    public class Reply : AggregateRoot<string>
    {
        public string PostId { get; private set; }
        public string ParentId { get; private set; }
        public string AuthorId { get; private set; }
        public string Body { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Reply(string id, string postId, string parentId, string authorId, string body) : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            Assert.IsNotNullOrWhiteSpace("被回复的帖子", postId);
            RaiseEvent(new ReplyCreatedEvent(Id, postId, parentId, authorId, body, DateTime.Now));
        }

        public void UpdateBody(string body)
        {
            if (Body == body) return;
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            RaiseEvent(new ReplyBodyUpdatedEvent(Id, body));
        }

        private void Handle(ReplyCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            PostId = evnt.PostId;
            ParentId = evnt.ParentId;
            Body = evnt.Body;
            AuthorId = evnt.AuthorId;
            CreatedOn = evnt.CreatedOn;
        }
        private void Handle(ReplyBodyUpdatedEvent evnt)
        {
            Body = evnt.Body;
        }
    }
}
