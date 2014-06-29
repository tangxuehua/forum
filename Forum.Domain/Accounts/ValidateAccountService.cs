using ECommon.Components;

namespace Forum.Domain.Accounts
{
    /// <summary>验证账号是否合法的领域服务
    /// </summary>
    [Component(LifeStyle.Singleton)]
    public class ValidateAccountService
    {
        private IAccountIndexStore _accountIndexStore;

        public ValidateAccountService(IAccountIndexStore accountIndexStore)
        {
            _accountIndexStore = accountIndexStore;
        }

        /// <summary>验证给定的账号名是否重复
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountId"></param>
        public AccountNameUniquenessValidateResult ValidateAccountNameUniqueness(string accountName, string accountId)
        {
            var result = _accountIndexStore.AddAccountNameIndex(accountName, accountId);
            if (result == AccountNameUniquenessValidateResult.DuplicateAccountName)
            {
                //这里，发现账号名称重复时，需要再查一下是否是同一个账号ID的账号；如果是，则认为没有重复。
                //出现这种情况的原因是有时注册新账号的命令被重复执行了。
                var existingAccountId = _accountIndexStore.GetAccountId(accountName);
                if (existingAccountId == accountId)
                {
                    result = AccountNameUniquenessValidateResult.Success;
                }
            }
            return result;
        }
    }
    public enum AccountNameUniquenessValidateResult
    {
        Success = 1,
        DuplicateAccountName
    }
}
