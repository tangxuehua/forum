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
        private readonly RegisterAccountIndexService _registerAccountIndexService;

        public AccountCommandHandler(ILockService lockService, RegisterAccountIndexService registerAccountIndexService)
        {
            _lockService = lockService;
            _registerAccountIndexService = registerAccountIndexService;
        }

        public void Handle(ICommandContext context, RegisterNewAccountCommand command)
        {
            _lockService.ExecuteInLock(typeof(Account).Name, () =>
            {
                _registerAccountIndexService.RegisterAccountIndex(command.AggregateRootId, command.Name);
                context.Add(new Account(command.AggregateRootId, command.Name, command.Password));
            });
        }
    }
}
