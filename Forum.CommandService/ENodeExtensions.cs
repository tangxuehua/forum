using ECommon.Components;
using ECommon.Extensions;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using ENode.EQueue;
using ENode.Eventing;
using EQueue.Configurations;
using Forum.CommandService.Providers;

namespace Forum.CommandService
{
    public static class ENodeExtensions
    {
        private static CommandConsumer _commandConsumer;
        private static EventPublisher _eventPublisher;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<ITopicProvider<ICommand>, CommandTopicProvider>();
            configuration.SetDefault<ICommandTypeCodeProvider, CommandTypeCodeProvider>();
            configuration.SetDefault<IAggregateRootTypeCodeProvider, AggregateRootTypeCodeProvider>();
            configuration.SetDefault<ITopicProvider<IDomainEvent>, EventTopicProvider>();
            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _eventPublisher = new EventPublisher();
            configuration.SetDefault<IEventPublisher, EventPublisher>(_eventPublisher);

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
