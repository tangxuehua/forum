using ENode.Domain;
using ENode.EQueue;
using ENode.Eventing;

namespace Forum.Api.Providers
{
    public class EventTopicProvider : IEventTopicProvider
    {
        private IAggregateRootTypeCodeProvider _aggregateRootTypeCodeProvider;

        public EventTopicProvider(IAggregateRootTypeCodeProvider aggregateRootTypeCodeProvider)
        {
            _aggregateRootTypeCodeProvider = aggregateRootTypeCodeProvider;
        }
        public string GetTopic(EventStream eventStream)
        {
            var aggregateRootTypeFullName = _aggregateRootTypeCodeProvider.GetType(eventStream.AggregateRootTypeCode).FullName;
            return aggregateRootTypeFullName.Substring(aggregateRootTypeFullName.LastIndexOf('.') + 1) + "EventTopic";
        }
    }
}
