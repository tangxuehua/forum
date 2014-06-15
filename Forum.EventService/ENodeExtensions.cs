using System.Linq;
using ECommon.Components;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using EQueue.Clients.Consumers;
using EQueue.Configurations;
using Forum.EventService.Providers;

namespace Forum.EventService
{
    public static class ENodeExtensions
    {
        private static EventConsumer _eventConsumer;
        private static DomainEventHandledMessageSender _domainEventHandledMessageSender;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<IEventTopicProvider, EventTopicProvider>();
            configuration.SetDefault<IEventTypeCodeProvider, EventTypeCodeProvider>();
            configuration.SetDefault<IEventHandlerTypeCodeProvider, EventHandlerTypeCodeProvider>();
            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _domainEventHandledMessageSender = new DomainEventHandledMessageSender();

            var consumerSetting = new ConsumerSetting
            {
                PullRequestSetting = new PullRequestSetting { PullRequestTimeoutMilliseconds = 7000 }
            };
            _eventConsumer = new EventConsumer(consumerSetting, _domainEventHandledMessageSender);

            var eventTopicProvider = ObjectContainer.Resolve<IEventTopicProvider>() as EventTopicProvider;

            eventTopicProvider.GetAllEventTopics().ToList().ForEach(topic => _eventConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Start();
            _domainEventHandledMessageSender.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            _domainEventHandledMessageSender.Shutdown();

            return enodeConfiguration;
        }
    }
}
