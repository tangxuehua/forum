using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class AccountCommandHandler : ICommandHandler<CreateAccountCommand>
    {
        private readonly AggregateRootFactory _factory;

        public AccountCommandHandler(AggregateRootFactory factory)
        {
            _factory = factory;
        }

        public void Handle(ICommandContext context, CreateAccountCommand command)
        {
            context.Add(_factory.CreateAccount(command.Name, command.Password));
        }
    }
}
