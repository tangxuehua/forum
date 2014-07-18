using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
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
                context.Add(_registerAccountService.RegisterNewAccount(command.Id, command.Name, command.Password));
            });
        }
    }
}
