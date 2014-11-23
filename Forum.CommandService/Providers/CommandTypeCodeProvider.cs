using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure.Impl;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;

namespace Forum.CommandService.Providers
{
    [Component]
    public class CommandTypeCodeProvider : DefaultTypeCodeProvider<ICommand>
    {
        public CommandTypeCodeProvider()
        {
            RegisterType<RegisterNewAccountCommand>(100);

            RegisterType<CreateSectionCommand>(200);
            RegisterType<ChangeSectionNameCommand>(201);

            RegisterType<CreatePostCommand>(300);
            RegisterType<UpdatePostCommand>(301);

            RegisterType<CreateReplyCommand>(400);
            RegisterType<ChangeReplyBodyCommand>(401);
        }
    }
}
