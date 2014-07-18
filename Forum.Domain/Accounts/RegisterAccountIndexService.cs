using ECommon.Components;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    /// <summary>领域服务，提供注册账号索引服务
    /// </summary>
    [Component(LifeStyle.Singleton)]
    public class RegisterAccountIndexService
    {
        private readonly IIdentityGenerator _identityGenerator;
        private readonly IAccountIndexStore _accountIndexStore;

        public RegisterAccountIndexService(IIdentityGenerator identityGenerator, IAccountIndexStore accountIndexStore)
        {
            _identityGenerator = identityGenerator;
            _accountIndexStore = accountIndexStore;
        }

        /// <summary>注册账号索引
        /// </summary>
        /// <param name="accountIndexId"></param>
        /// <param name="accountId"></param>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public void RegisterAccountIndex(string accountIndexId, string accountId, string accountName)
        {
            var accountIndex = _accountIndexStore.FindByAccountName(accountName);
            if (accountIndex == null)
            {
                _accountIndexStore.Add(new AccountIndex(accountIndexId, accountId, accountName));
            }
            else if (accountIndex.IndexId != accountIndexId)
            {
                throw new DuplicateAccountException(accountName);
            }
        }
    }
}
