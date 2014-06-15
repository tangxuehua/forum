using System.Linq;
using ECommon.Components;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using ENode.EQueue;
using ENode.Eventing;
using EQueue.Clients.Consumers;
using EQueue.Configurations;
using Forum.CommandService.Providers;

namespace Forum.CommandService
{
    public static class ENodeExtensions
    {
        private static CommandConsumer _commandConsumer;
        private static EventPublisher _eventPublisher;
        private static CommandExecutedMessageSender _commandExecutedMessageSender;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<ICommandTopicProvider, CommandTopicProvider>();
            configuration.SetDefault<ICommandTypeCodeProvider, CommandTypeCodeProvider>();
            configuration.SetDefault<IAggregateRootTypeCodeProvider, AggregateRootTypeCodeProvider>();
            configuration.SetDefault<IEventTopicProvider, EventTopicProvider>();
            configuration.SetDefault<IEventTypeCodeProvider, EventTypeCodeProvider>();
            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _commandExecutedMessageSender = new CommandExecutedMessageSender();
            _eventPublisher = new EventPublisher();

            configuration.SetDefault<IEventPublisher, EventPublisher>(_eventPublisher);

            var consumerSetting = new ConsumerSetting
            {
                PullRequestSetting = new PullRequestSetting { PullRequestTimeoutMilliseconds = 7000 }
            };

            _commandConsumer = new CommandConsumer(consumerSetting, _commandExecutedMessageSender);

            var commandTopicProvider = ObjectContainer.Resolve<ICommandTopicProvider>() as CommandTopicProvider;

            commandTopicProvider.GetAllCommandTopics().ToList().ForEach(topic => _commandConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandConsumer.Start();
            _eventPublisher.Start();
            _commandExecutedMessageSender.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandConsumer.Shutdown();
            _eventPublisher.Shutdown();
            _commandExecutedMessageSender.Shutdown();

            return enodeConfiguration;
        }
    }
}
