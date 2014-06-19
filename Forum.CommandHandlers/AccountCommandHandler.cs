using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class AccountCommandHandler : ICommandHandler<CreateAccountCommand>
    {
        private readonly AccountFactory _factory;

        public AccountCommandHandler(AccountFactory factory)
        {
            _factory = factory;
        }

        public void Handle(ICommandContext context, CreateAccountCommand command)
        {
            context.Add(_factory.CreateAccount(command.Name, command.Password));
        }
    }
}
