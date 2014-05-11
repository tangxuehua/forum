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

            var consumerSetting = new ConsumerSetting
            {
                HeartbeatBrokerInterval = 1000,
                UpdateTopicQueueCountInterval = 1000,
                RebalanceInterval = 1000,
                MessageHandleMode = MessageHandleMode.Sequential
            };

            _domainEventHandledMessageSender = new DomainEventHandledMessageSender();

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
