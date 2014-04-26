using Forum.Domain.Accounts;

namespace Forum.QueryServices
{
    public interface IAccountService
    {
        /// <summary>根据账号名获取账号
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        Account GetAccount(string accountName);
    }
}
