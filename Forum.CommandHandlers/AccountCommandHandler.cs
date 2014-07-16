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
        private readonly IAccountIndexStore _indexStore;

        public AccountCommandHandler(AggregateRootFactory factory, ILockService lockService, IAccountIndexStore indexStore)
        {
            _factory = factory;
            _lockService = lockService;
            _indexStore = indexStore;
        }

        public void Handle(ICommandContext context, RegisterNewAccountCommand command)
        {
            _lockService.ExecuteInLock(typeof(Account).Name, () =>
            {
                var sectionIndex = _indexStore.FindByAccountName(command.Name);
                if (sectionIndex == null)
                {
                    var account = _factory.CreateAccount(command.Name, command.Password);
                    _indexStore.Add(new AccountIndex(command.Id, account.Id, command.Name));
                    context.Add(account);
                }
                else if (sectionIndex.IndexId == command.Id)
                {
                    context.Add(_factory.CreateAccount(command.Name, command.Password));
                }
                else
                {
                    throw new DuplicateAccountException(command.Name);
                }
            });
        }
    }
}
