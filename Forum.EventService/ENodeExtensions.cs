using ECommon.Components;
using ECommon.Extensions;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Messaging;
using EQueue.Configurations;

namespace Forum.EventService
{
    public static class ENodeExtensions
    {
        private static EventConsumer _eventConsumer;
        private static MessagePublisher _messagePublisher;
        private static MessageConsumer _messageConsumer;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _messagePublisher = new MessagePublisher();
            configuration.SetDefault<IPublisher<IMessage>, MessagePublisher>(_messagePublisher);

            _eventConsumer = new EventConsumer();
            _messageConsumer = new MessageConsumer();

            ObjectContainer.Resolve<ITopicProvider<IEvent>>().GetAllTopics().ForEach(topic => _eventConsumer.Subscribe(topic));
            ObjectContainer.Resolve<ITopicProvider<IMessage>>().GetAllTopics().ForEach(topic => _messageConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Start();
            _messagePublisher.Start();
            _messageConsumer.Start();
            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            _messagePublisher.Shutdown();
            _messageConsumer.Shutdown();
            return enodeConfiguration;
        }
    }
}
