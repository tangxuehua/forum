using ECommon.Components;
using ENode.EQueue;
using ENode.Eventing;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.CommandService.Providers
{
    [Component]
    public class EventTopicProvider : AbstractTopicProvider<IDomainEvent>
    {
        public EventTopicProvider()
        {
            RegisterTopic("AccountEventTopic", typeof(NewAccountRegisteredEvent));
            RegisterTopic("SectionEventTopic", typeof(SectionCreatedEvent), typeof(SectionChangedEvent));
            RegisterTopic("PostEventTopic", typeof(PostCreatedEvent), typeof(PostUpdatedEvent), typeof(PostReplyStatisticInfoChangedEvent));
            RegisterTopic("ReplyEventTopic", typeof(ReplyCreatedEvent), typeof(ReplyBodyChangedEvent));
        }
    }
}
