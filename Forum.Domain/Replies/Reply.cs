using System;
using ENode.Domain;
using Forum.Infrastructure;

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

        public Reply(string id, string postId, Reply parent, string authorId, string body)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("被回复的帖子", postId);
            Assert.IsNotNullOrWhiteSpace("回复作者", authorId);
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            if (parent != null && id == parent.Id)
            {
                throw new ArgumentException(string.Format("回复的parentId不能是当前回复的Id:{0}", id));
            }
            RaiseEvent(new ReplyCreatedEvent(Id, postId, parent == null ? null : parent.Id, authorId, body, DateTime.Now));
        }

        public void ChangeBody(string body)
        {
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            RaiseEvent(new ReplyBodyChangedEvent(Id, body));
        }

        private void Handle(ReplyCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            PostId = evnt.PostId;
            ParentId = evnt.ParentId;
            Body = evnt.Body;
            AuthorId = evnt.AuthorId;
        }
        private void Handle(ReplyBodyChangedEvent evnt)
        {
            Body = evnt.Body;
        }
    }
}
