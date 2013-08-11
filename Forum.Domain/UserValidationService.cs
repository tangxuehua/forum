using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Events;

namespace Forum.Domain {
    [Component]
    public class UserValidationService : IEventPersistenceSynchronizer<AccountCreated> {

        private IAccountRepository _accountRepository;

        public UserValidationService(IAccountRepository accountRepository) {
            _accountRepository = accountRepository;
        }

        public void OnBeforePersisting(AccountCreated evnt) {
            _accountRepository.AddAccount(evnt.AccountId, evnt.Name);
        }
        public void OnAfterPersisted(AccountCreated evnt) {
            _accountRepository.ConfirmAccount(evnt.AccountId);
        }
    }
}
