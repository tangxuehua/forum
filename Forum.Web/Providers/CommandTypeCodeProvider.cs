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
            RegisterType<StartRegistrationCommand>(100);
            RegisterType<ConfirmRegistrationCommand>(101);
            RegisterType<CancelRegistrationCommand>(102);
            RegisterType<CompleteRegistrationCommand>(103);

            RegisterType<CreateAccountCommand>(200);

            RegisterType<CreateSectionCommand>(300);
            RegisterType<UpdateSectionCommand>(301);

            RegisterType<CreatePostCommand>(400);
            RegisterType<UpdatePostCommand>(401);

            RegisterType<CreateReplyCommand>(500);
            RegisterType<UpdateReplyBodyCommand>(501);
        }
    }
}
