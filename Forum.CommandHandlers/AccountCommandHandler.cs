using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Domain;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class AccountCommandHandler :
        ICommandHandler<RegisterNewAccountCommand>
    {
        private readonly AggregateRootFactory _factory;
        private readonly ILockService _lockService;

        public AccountCommandHandler(AggregateRootFactory factory, ILockService lockService)
        {
            _factory = factory;
            _lockService = lockService;
        }

        public void Handle(ICommandContext context, RegisterNewAccountCommand command)
        {
            _lockService.ExecuteInLock(typeof(Account).Name, () =>
            {
                context.Add(_factory.CreateAccount(command.Id, command.Name, command.Password));
            });
        }
    }
}
