using ECommon.Components;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using EQueue.Configurations;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.CommandService
{
    public static class ENodeExtensions
    {
        private static CommandConsumer _commandConsumer;
        private static DomainEventPublisher _eventPublisher;

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            //aggregates
            provider.RegisterType<Account>(10);
            provider.RegisterType<Section>(11);
            provider.RegisterType<Post>(12);
            provider.RegisterType<Reply>(13);

            //commands
            provider.RegisterType<RegisterNewAccountCommand>(11000);

            provider.RegisterType<CreateSectionCommand>(11100);
            provider.RegisterType<ChangeSectionNameCommand>(11101);

            provider.RegisterType<CreatePostCommand>(11200);
            provider.RegisterType<UpdatePostCommand>(11201);
            provider.RegisterType<AcceptNewReplyCommand>(11202);

            provider.RegisterType<CreateReplyCommand>(11300);
            provider.RegisterType<ChangeReplyBodyCommand>(11301);

            //domain events
            provider.RegisterType<NewAccountRegisteredEvent>(21000);

            provider.RegisterType<SectionCreatedEvent>(21100);
            provider.RegisterType<SectionNameChangedEvent>(21101);

            provider.RegisterType<PostCreatedEvent>(21200);
            provider.RegisterType<PostUpdatedEvent>(21201);
            provider.RegisterType<PostReplyStatisticInfoChangedEvent>(21202);

            provider.RegisterType<ReplyCreatedEvent>(21300);
            provider.RegisterType<ReplyBodyChangedEvent>(21301);

            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _eventPublisher = new DomainEventPublisher();
            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_eventPublisher);

            _commandConsumer = new CommandConsumer();
            _commandConsumer
                .Subscribe("AccountCommandTopic")
                .Subscribe("SectionCommandTopic")
                .Subscribe("PostCommandTopic")
                .Subscribe("ReplyCommandTopic");

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
