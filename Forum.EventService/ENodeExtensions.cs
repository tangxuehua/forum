using ECommon.Components;
using ECommon.Extensions;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using EQueue.Configurations;
using Forum.EventService.Providers;

namespace Forum.EventService
{
    public static class ENodeExtensions
    {
        private static EventConsumer _eventConsumer;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<ITopicProvider<IDomainEvent>, EventTopicProvider>();
            configuration.SetDefault<IEventTypeCodeProvider, EventTypeCodeProvider>();
            configuration.SetDefault<IEventHandlerTypeCodeProvider, EventHandlerTypeCodeProvider>();
            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _eventConsumer = new EventConsumer();

            ObjectContainer.Resolve<ITopicProvider<IDomainEvent>>().GetAllTopics().ForEach(topic => _eventConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Start();
            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            return enodeConfiguration;
        }
    }
}
