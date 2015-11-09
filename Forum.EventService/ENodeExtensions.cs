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

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _commandService = new CommandService();
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
