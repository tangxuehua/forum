using ECommon.Components;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using EQueue.Configurations;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;

namespace Forum.Web.Extensions
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            //commands
            provider.RegisterType<RegisterNewAccountCommand>(11000);

            provider.RegisterType<CreateSectionCommand>(11100);
            provider.RegisterType<ChangeSectionNameCommand>(11101);

            provider.RegisterType<CreatePostCommand>(11200);
            provider.RegisterType<UpdatePostCommand>(11201);
            provider.RegisterType<AcceptNewReplyCommand>(11202);

            provider.RegisterType<CreateReplyCommand>(11300);
            provider.RegisterType<ChangeReplyBodyCommand>(11301);

            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _commandService = new CommandService(new CommandResultProcessor());

            configuration.SetDefault<ICommandService, CommandService>(_commandService);

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Start();
            return enodeConfiguration;
        }
    }
}