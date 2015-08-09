using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component]
    public class AccountCommandHandler :
        ICommandHandler<RegisterNewAccountCommand>
    {
        private readonly ILockService _lockService;
        private readonly RegisterAccountService _registerAccountService;

        public AccountCommandHandler(ILockService lockService, RegisterAccountService registerAccountService)
        {
            _lockService = lockService;
            _registerAccountService = registerAccountService;
        }

        public void Handle(ICommandContext context, RegisterNewAccountCommand command)
        {
            _lockService.ExecuteInLock(typeof(Account).Name, () =>
            {
                _registerAccountService.RegisterAccount(command.AggregateRootId, command.Name);
                context.Add(new Account(command.AggregateRootId, command.Name, command.Password));
            });
        }
    }
}
