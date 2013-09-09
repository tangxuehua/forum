using System;
using ENode.Domain;
using ENode.Eventing;
using Forum.Domain.Posts.Model;
using Forum.Domain.Replies.Events;

namespace Forum.Domain.Replies.Model
{
    /// <summary>回复聚合根
    /// </summary>
    [Serializable]
    public class Reply : AggregateRoot<Guid>,
        IEventHandler<PostReplied>,
        IEventHandler<ReplyReplied>,
        IEventHandler<ReplyBodyChanged>
    {
        public Guid ParentId { get; private set; }
        public Guid PostId { get; private set; }
        public string Body { get; private set; }
        public Guid AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Reply() { }
        public Reply(string body, Post post, Guid authorId) : base(Guid.NewGuid())
        {
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            Assert.IsNotNull("被回复的帖子", post);
            RaiseEvent(new PostReplied(Id, post.Id, body, authorId, DateTime.Now));
        }
        public Reply(string body, Reply reply, Guid authorId) : base(Guid.NewGuid())
        {
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            Assert.IsNotNull("被回复的回复", reply);
            RaiseEvent(new ReplyReplied(Id, reply.Id, reply.PostId, body, authorId, DateTime.Now));
        }

        public void ChangeBody(string body)
        {
            if (Body == body) return;
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            RaiseEvent(new ReplyBodyChanged(Id, body));
        }

        void IEventHandler<PostReplied>.Handle(PostReplied evnt)
        {
            PostId = evnt.PostId;
            Body = evnt.Body;
            AuthorId = evnt.AuthorId;
            CreatedOn = evnt.CreatedOn;
        }
        void IEventHandler<ReplyReplied>.Handle(ReplyReplied evnt)
        {
            ParentId = evnt.ParentReplyId;
            PostId = evnt.PostId;
            Body = evnt.Body;
            AuthorId = evnt.AuthorId;
            CreatedOn = evnt.CreatedOn;
        }
        void IEventHandler<ReplyBodyChanged>.Handle(ReplyBodyChanged evnt)
        {
            Body = evnt.Body;
        }
    }
}
