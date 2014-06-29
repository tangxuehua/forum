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
using EQueueCommandService = ENode.EQueue.CommandService;

namespace Forum.CommandService
{
    public static class ENodeExtensions
    {
        private static EQueueCommandService _commandService;
        private static CommandConsumer _commandConsumer;
        private static EventPublisher _eventPublisher;
        private static EventConsumer _eventConsumer;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<ICommandTopicProvider, CommandTopicProvider>();
            configuration.SetDefault<ICommandTypeCodeProvider, CommandTypeCodeProvider>();
            configuration.SetDefault<IAggregateRootTypeCodeProvider, AggregateRootTypeCodeProvider>();
            configuration.SetDefault<IEventTopicProvider, EventTopicProvider>();
            configuration.SetDefault<IEventTypeCodeProvider, EventTypeCodeProvider>();
            configuration.SetDefault<IEventHandlerTypeCodeProvider, EventHandlerTypeCodeProvider>();
            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _commandService = new EQueueCommandService();
            configuration.SetDefault<ICommandService, EQueueCommandService>(_commandService);

            _eventPublisher = new EventPublisher();
            configuration.SetDefault<IEventPublisher, EventPublisher>(_eventPublisher);

            _commandConsumer = new CommandConsumer();
            var commandTopicProvider = ObjectContainer.Resolve<ICommandTopicProvider>() as CommandTopicProvider;
            commandTopicProvider.GetAllCommandTopics().ToList().ForEach(topic => _commandConsumer.Subscribe(topic));

            _eventConsumer = new EventConsumer(id: "SagaEventConsumer", groupName: "SagaEventConsumerGroup", domainEventHandledMessageSender: new DomainEventHandledMessageSender("sys_dehmsp_saga"));
            var eventTopicProvider = ObjectContainer.Resolve<IEventTopicProvider>() as EventTopicProvider;
            eventTopicProvider.GetAllEventTopics().ToList().ForEach(topic => _eventConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();
            _eventConsumer.Start();
            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Shutdown();
            _commandConsumer.Shutdown();
            _eventPublisher.Shutdown();
            _eventConsumer.Shutdown();
            return enodeConfiguration;
        }
    }
}
