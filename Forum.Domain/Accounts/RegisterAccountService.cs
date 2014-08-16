using ECommon.Components;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    /// <summary>提供账号注册的领域服务，封装账号注册的业务规则，比如账号唯一性检查
    /// </summary>
    [Component(LifeStyle.Singleton)]
    public class RegisterAccountService
    {
        private readonly AggregateRootFactory _factory;
        private readonly IAccountIndexStore _accountIndexStore;

        public RegisterAccountService(AggregateRootFactory factory, IAccountIndexStore accountIndexStore)
        {
            _factory = factory;
            _accountIndexStore = accountIndexStore;
        }

        /// <summary>注册新账号
        /// </summary>
        /// <param name="accountIndexId"></param>
        /// <param name="accountName"></param>
        /// <param name="accountPassword"></param>
        /// <returns></returns>
        public Account RegisterNewAccount(string accountIndexId, string accountName, string accountPassword)
        {
            //首先创建一个新账号
            var account = _factory.CreateAccount(accountName, accountPassword);

            //先判断该账号是否存在
            var accountIndex = _accountIndexStore.FindByAccountName(account.Name);
            if (accountIndex == null)
            {
                //如果不存在，则添加到账号索引
                _accountIndexStore.Add(new AccountIndex(accountIndexId, account.Id, account.Name));
            }
            else if (accountIndex.IndexId != accountIndexId)
            {
                //如果存在但和当前的索引ID不同，则认为是账号有重复
                throw new DuplicateAccountException(accountName);
            }

            return account;
        }
    }
}
