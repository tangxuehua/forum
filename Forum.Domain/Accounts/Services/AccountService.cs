using ENode.Domain;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Accounts.Events;
using Forum.Domain.Accounts.Model;
using Forum.Domain.Accounts.Repositories;

namespace Forum.Domain.Accounts.Services
{
    [Component]
    public class AccountService : IAccountService, IEventSynchronizer<AccountCreated>
    {
        private readonly IRepository _repository;
        private readonly IAccountRegistrationInfoRepository _accountRegistrationInfoRepository;

        public AccountService(IRepository repository, IAccountRegistrationInfoRepository accountRegistrationInfoRepository)
        {
            _repository = repository;
            _accountRegistrationInfoRepository = accountRegistrationInfoRepository;
        }

        public Accounts.Model.Account GetAccount(string accountName)
        {
            var accountRegistrationInfo = _accountRegistrationInfoRepository.Get(accountName);
            if (accountRegistrationInfo != null &&
                accountRegistrationInfo.RegistrationStatus == AccountRegistrationStatus.Confirmed)
            {
                return _repository.Get<Accounts.Model.Account>(accountRegistrationInfo.AccountId);
            }
            return null;
        }

        void IEventSynchronizer<AccountCreated>.OnBeforePersisting(AccountCreated evnt)
        {
            _accountRegistrationInfoRepository.Add(new AccountRegistrationInfo(evnt.AccountId, evnt.Name));
        }
        void IEventSynchronizer<AccountCreated>.OnAfterPersisted(AccountCreated evnt)
        {
            var registrationInfo = _accountRegistrationInfoRepository.Get(evnt.Name);
            registrationInfo.ConfirmStatus();
            _accountRegistrationInfoRepository.Update(registrationInfo);
        }
    }
}
