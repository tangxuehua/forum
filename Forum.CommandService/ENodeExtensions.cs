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
