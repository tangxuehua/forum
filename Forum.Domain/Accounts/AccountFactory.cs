using ECommon.Components;

namespace Forum.Domain.Accounts
{
    [Component(LifeStyle.Singleton)]
    public class AccountFactory
    {
        private IAccountRepository _repository;

        public AccountFactory(IAccountRepository repository)
        {
            _repository = repository;
        }

        public Account CreateAccount(string name, string password)
        {
            return new Account(_repository.GetNextAccountId(), name, password);
        }
    }
}
