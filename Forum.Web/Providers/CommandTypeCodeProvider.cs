using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;

namespace Forum.Web.Providers
{
    public class CommandTypeCodeProvider : AbstractTypeCodeProvider, ICommandTypeCodeProvider
    {
        public CommandTypeCodeProvider()
        {
            RegisterType<CreateAccountCommand>(100);

            RegisterType<CreateSectionCommand>(200);
            RegisterType<UpdateSectionCommand>(201);

            RegisterType<CreatePostCommand>(300);
            RegisterType<UpdatePostCommand>(301);

            RegisterType<CreateReplyCommand>(400);
            RegisterType<UpdateReplyBodyCommand>(401);
        }
    }
}
