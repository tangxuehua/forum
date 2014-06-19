using ECommon.Components;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    /// <summary>一个规则服务，用于检查账号是否唯一
    /// </summary>
    [Component(LifeStyle.Singleton)]
    public class CheckAccountNameUniqueService : IEventSynchronizer<AccountCreatedEvent>
    {
        private readonly IAccountRepository _accountRepository;

        public CheckAccountNameUniqueService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void OnBeforePersisting(AccountCreatedEvent evnt)
        {
            //_accountRepository.Add(new Registration(evnt.AggregateRootId, evnt.Name));
        }
        public void OnAfterPersisted(AccountCreatedEvent evnt)
        {
        }
    }
}
