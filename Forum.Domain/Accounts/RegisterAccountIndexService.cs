using ECommon.Components;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    /// <summary>注册账号索引领域服务，封装账号注册时关于账号名唯一的业务规则
    /// </summary>
    [Component]
    public class RegisterAccountIndexService
    {
        private readonly IAccountIndexRepository _accountIndexRepository;

        public RegisterAccountIndexService(IAccountIndexRepository accountIndexRepository)
        {
            _accountIndexRepository = accountIndexRepository;
        }

        /// <summary>注册账号索引
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public void RegisterAccountIndex(string accountId, string accountName)
        {
            var accountIndex = _accountIndexRepository.FindByAccountName(accountName);
            if (accountIndex == null)
            {
                _accountIndexRepository.Add(new AccountIndex(accountId, accountName));
            }
            else if (accountIndex.AccountId != accountId)
            {
                throw new DuplicateAccountException(accountName);
            }
        }
    }
}
