using System;
using ENode.Domain;
using Forum.Domain.Posts;
using Forum.Events.Reply;

namespace Forum.Domain.Replies
{
    /// <summary>回复聚合根
    /// </summary>
    [Serializable]
    public class Reply : AggregateRoot<string>
    {
        public string ParentId { get; private set; }
        public string PostId { get; private set; }
        public string Body { get; private set; }
        public string AuthorId { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Reply(string id, string body, Post post, string authorId) : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            Assert.IsNotNull("被回复的帖子", post);
            RaiseEvent(new PostReplied(new RepliedEventInfo(Id, post.Id, body, authorId, DateTime.Now)));
        }
        public Reply(string id, string body, Reply parent, string authorId) : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            Assert.IsNotNull("被回复的回复", parent);
            RaiseEvent(new ReplyReplied(parent.Id, new RepliedEventInfo(Id, parent.PostId, body, authorId, DateTime.Now)));
        }

        public void ChangeBody(string body)
        {
            if (Body == body) return;
            Assert.IsNotNullOrWhiteSpace("回复内容", body);
            RaiseEvent(new ReplyBodyChanged(Id, body));
        }

        private void Handle(PostReplied evnt)
        {
            Id = evnt.AggregateRootId;
            PostId = evnt.Info.PostId;
            Body = evnt.Info.Body;
            AuthorId = evnt.Info.AuthorId;
            CreatedOn = evnt.Info.CreatedOn;
        }
        private void Handle(ReplyReplied evnt)
        {
            Id = evnt.AggregateRootId;
            ParentId = evnt.ParentReplyId;
            PostId = evnt.Info.PostId;
            Body = evnt.Info.Body;
            AuthorId = evnt.Info.AuthorId;
            CreatedOn = evnt.Info.CreatedOn;
        }
        private void Handle(ReplyBodyChanged evnt)
        {
            Body = evnt.Body;
        }
    }
}
