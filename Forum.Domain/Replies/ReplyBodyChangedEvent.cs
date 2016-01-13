using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Replies
{
    /// <summary>表示回复内容已修改的领域事件
    /// </summary>
    public class ReplyBodyChangedEvent : DomainEvent<string>
    {
        public string Body { get; private set; }

        private ReplyBodyChangedEvent() { }
        public ReplyBodyChangedEvent( string body) 
        {
            Body = body;
        }
    }
}
