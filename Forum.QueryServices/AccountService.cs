using ECommon.IoC;
using ENode.Domain;
using Forum.Domain.Accounts;

namespace Forum.QueryServices
{
    [Component]
    public class AccountService : IAccountService
    {
        private readonly IRepository _repository;
        private readonly IRegistrationRepository _registrationRepository;

        public AccountService(IRepository repository, IRegistrationRepository registrationRepository)
        {
            _repository = repository;
            _registrationRepository = registrationRepository;
        }

        public Account GetAccount(string accountName)
        {
            var registration = _registrationRepository.GetByAccountName(accountName);
            if (registration != null && registration.RegistrationStatus == RegistrationStatus.Confirmed)
            {
                return _repository.Get<Account>(registration.AccountId);
            }
            return null;
        }
    }
}
