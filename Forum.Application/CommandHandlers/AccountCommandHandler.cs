using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Application.Commands;
using Forum.Domain.Model;

namespace Forum.Application.CommandHandlers
{
    [Component]
    public class AccountCommandHandler : ICommandHandler<CreateAccount>
    {
        public void Handle(ICommandContext context, CreateAccount command)
        {
            context.Add(new Account(command.Name, command.Password));
        }
    }
}
