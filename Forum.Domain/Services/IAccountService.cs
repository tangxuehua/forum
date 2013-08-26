using Forum.Domain.Model.Account;

namespace Forum.Domain.Services
{
    public interface IAccountService
    {
        /// <summary>根据用户名获取账号
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        Account GetAccount(string accountName);
    }
}
