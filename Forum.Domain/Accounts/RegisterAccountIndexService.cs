using ECommon.Components;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    /// <summary>注册账号索引领域服务，封装账号注册时关于账号名唯一的业务规则
    /// </summary>
    [Component(LifeStyle.Singleton)]
    public class RegisterAccountIndexService
    {
        private readonly IAccountIndexRepository _accountIndexRepository;

        public RegisterAccountIndexService(IAccountIndexRepository accountIndexRepository)
        {
            _accountIndexRepository = accountIndexRepository;
        }

        /// <summary>注册账号索引
        /// </summary>
        /// <param name="indexId"></param>
        /// <param name="accountId"></param>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public void RegisterAccountIndex(string indexId, string accountId, string accountName)
        {
            var accountIndex = _accountIndexRepository.FindByAccountName(accountName);
            if (accountIndex == null)
            {
                _accountIndexRepository.Add(new AccountIndex(indexId, accountId, accountName));
            }
            else if (accountIndex.IndexId != indexId)
            {
                throw new DuplicateAccountException(accountName);
            }
        }
    }
}
