using System;
using ENode.Eventing;

namespace Forum.Events.Reply
{
    /// <summary>表示回复已创建的领域事件
    /// </summary>
    [Serializable]
    public class ReplyCreatedEvent : DomainEvent<string>
    {
        public string PostId { get; private set; }
        public string ParentId { get; private set; }
        public string AuthorId { get; private set; }
        public string Body { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public ReplyCreatedEvent(string id, string postId, string parentId, string authorId, string body, DateTime createdOn) : base(id)
        {
            PostId = postId;
            ParentId = parentId;
            AuthorId = authorId;
            Body = body;
            CreatedOn = createdOn;
        }
    }
}