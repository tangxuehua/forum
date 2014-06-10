using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Eventing;
using EQueue.Clients.Consumers;
using EQueue.Configurations;
using Forum.Web.Providers;

namespace Forum.Web.Extensions
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
        private static CommandResultProcessor _commandResultProcessor;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<ICommandTopicProvider, CommandTopicProvider>();
            configuration.SetDefault<ICommandTypeCodeProvider, CommandTypeCodeProvider>();
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
                RebalanceInterval = 1000
            };

            var commandExecutedMessageConsumer = new Consumer("CommandExecutedMessageConsumer", "CommandExecutedMessageConsumerGroup", consumerSetting);
            var domainEventHandledMessageConsumer = new Consumer("DomainEventHandledMessageConsumer", "DomainEventHandledMessageConsumerGroup", consumerSetting);
            _commandResultProcessor = new CommandResultProcessor(commandExecutedMessageConsumer, domainEventHandledMessageConsumer);

            _commandService = new CommandService(_commandResultProcessor);

            configuration.SetDefault<ICommandService, CommandService>(_commandService);

            _commandResultProcessor.SetExecutedCommandMessageTopic("ExecutedCommandMessageTopic");
            _commandResultProcessor.SetDomainEventHandledMessageTopic("DomainEventHandledMessageTopic");

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Start();
            _commandResultProcessor.Start();

            return enodeConfiguration;
        }
    }
}