using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;

namespace Forum.EventService.Providers
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
        }
    }
}
