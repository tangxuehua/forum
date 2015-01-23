using ECommon.Components;
using ECommon.Extensions;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using EQueue.Configurations;

namespace Forum.CommandService
{
    public static class ENodeExtensions
    {
        private static CommandConsumer _commandConsumer;
        private static EventPublisher _eventPublisher;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _eventPublisher = new EventPublisher();

            configuration.SetDefault<IPublisher<EventStream>, EventPublisher>(_eventPublisher);
            configuration.SetDefault<IPublisher<DomainEventStream>, EventPublisher>(_eventPublisher);

            _commandConsumer = new CommandConsumer();
            ObjectContainer.Resolve<ITopicProvider<ICommand>>().GetAllTopics().ForEach(topic => _commandConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandConsumer.Start();
            _eventPublisher.Start();
            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandConsumer.Shutdown();
            _eventPublisher.Shutdown();
            return enodeConfiguration;
        }
    }
}
