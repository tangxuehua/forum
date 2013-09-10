using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Account;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class AccountCommandHandler : ICommandHandler<CreateAccount>
    {
        public void Handle(ICommandContext context, CreateAccount command)
        {
            context.Add(new Account(command.Name, command.Password));
        }
    }
}
