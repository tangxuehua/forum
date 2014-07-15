using ENode.EQueue;
using ENode.Eventing;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.EventService.Providers
{
    public class EventTopicProvider : AbstractTopicProvider<IDomainEvent>
    {
        public EventTopicProvider()
        {
            RegisterTopic("AccountEventTopic", typeof(NewAccountRegisteredEvent));
            RegisterTopic("SectionEventTopic", typeof(SectionCreatedEvent), typeof(SectionNameChangedEvent));
            RegisterTopic("PostEventTopic", typeof(PostCreatedEvent), typeof(PostUpdatedEvent));
            RegisterTopic("ReplyEventTopic", typeof(ReplyCreatedEvent), typeof(ReplyBodyChangedEvent));
        }
    }
}
