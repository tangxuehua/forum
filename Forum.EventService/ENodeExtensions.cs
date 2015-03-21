using ECommon.Components;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using EQueue.Configurations;
using Forum.Commands.Posts;
using Forum.Denormalizers.Dapper;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;
using Forum.ProcessManagers;

namespace Forum.EventService
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
        private static DomainEventConsumer _eventConsumer;

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            //commands
            provider.RegisterType<AcceptNewReplyCommand>(11202);

            //domain events
            provider.RegisterType<NewAccountRegisteredEvent>(21000);

            provider.RegisterType<SectionCreatedEvent>(21100);
            provider.RegisterType<SectionNameChangedEvent>(21101);

            provider.RegisterType<PostCreatedEvent>(21200);
            provider.RegisterType<PostUpdatedEvent>(21201);
            provider.RegisterType<PostReplyStatisticInfoChangedEvent>(21202);

            provider.RegisterType<ReplyCreatedEvent>(21300);
            provider.RegisterType<ReplyBodyChangedEvent>(21301);

            //denormalizers
            provider.RegisterType<AccountDenormalizer>(110);
            provider.RegisterType<SectionDenormalizer>(111);
            provider.RegisterType<PostDenormalizer>(112);
            provider.RegisterType<ReplyDenormalizer>(113);

            //process managers and other event handlers
            provider.RegisterType<CreateReplyProcessManager>(1000);

            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _commandService = new CommandService(id: "CommandServiceForProcessManager");
            configuration.SetDefault<ICommandService, CommandService>(_commandService);

            _eventConsumer = new DomainEventConsumer();
            _eventConsumer
                .Subscribe("AccountEventTopic")
                .Subscribe("SectionEventTopic")
                .Subscribe("PostEventTopic")
                .Subscribe("ReplyEventTopic");

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Start();
            _eventConsumer.Start();
            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            _commandService.Shutdown();
            return enodeConfiguration;
        }
    }
}
