using System;
using System.Collections.Generic;
using System.Linq;
using ENode.EQueue;
using ENode.Eventing;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.CommandService.Providers
{
    public class EventTopicProvider : IEventTopicProvider
    {
        private IDictionary<Type, string> _topicDict = new Dictionary<Type, string>();

        public EventTopicProvider()
        {
            RegisterTopic("AccountEventTopic", typeof(NewAccountRegisteredEvent), typeof(AccountConfirmedEvent), typeof(AccountRejectedEvent));
        }

        public string GetTopic(EventStream eventStream)
        {
            return _topicDict[eventStream.Events.First().GetType()];
        }
        public IEnumerable<string> GetAllEventTopics()
        {
            return _topicDict.Values.Distinct();
        }

        private void RegisterTopic(string topic, params Type[] eventTypes)
        {
            foreach (var eventType in eventTypes)
            {
                _topicDict.Add(eventType, topic);
            }
        }
    }
}
