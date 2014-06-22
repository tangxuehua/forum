using System;
using ENode.Eventing;

namespace Forum.Domain.Replies
{
    /// <summary>表示回复内容已修改的领域事件
    /// </summary>
    [Serializable]
    public class ReplyBodyUpdatedEvent : DomainEvent<string>
    {
        public string Body { get; private set; }

        public ReplyBodyUpdatedEvent(string id, string body) : base(id)
        {
            Body = body;
        }
    }
}
