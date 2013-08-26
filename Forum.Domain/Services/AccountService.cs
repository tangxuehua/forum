using ENode.Domain;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Events;
using Forum.Domain.Model.Account;
using Forum.Domain.Repositories;

namespace Forum.Domain.Services
{
    [Component]
    public class AccountService : IAccountService, IEventPersistenceSynchronizer<AccountCreated>
    {
        private readonly IRepository _repository;
        private readonly IAccountRegistrationInfoRepository _accountRegistrationInfoRepository;

        public AccountService(IRepository repository, IAccountRegistrationInfoRepository accountRegistrationInfoRepository)
        {
            _repository = repository;
            _accountRegistrationInfoRepository = accountRegistrationInfoRepository;
        }

        public Account GetAccount(string accountName)
        {
            var accountRegistrationInfo = _accountRegistrationInfoRepository.Get(accountName);
            if (accountRegistrationInfo != null &&
                accountRegistrationInfo.RegistrationStatus == AccountRegistrationStatus.Confirmed)
            {
                return _repository.Get<Account>(accountRegistrationInfo.AccountId);
            }
            return null;
        }

        void IEventPersistenceSynchronizer<AccountCreated>.OnBeforePersisting(AccountCreated evnt)
        {
            _accountRegistrationInfoRepository.Add(new AccountRegistrationInfo(evnt.AccountId, evnt.Name, AccountRegistrationStatus.Created));
        }
        void IEventPersistenceSynchronizer<AccountCreated>.OnAfterPersisted(AccountCreated evnt)
        {
            var registrationInfo = _accountRegistrationInfoRepository.Get(evnt.Name);
            registrationInfo.ConfirmStatus();
            _accountRegistrationInfoRepository.Update(registrationInfo);
        }
    }
}
