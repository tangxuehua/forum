namespace Forum.Domain.Accounts.Services
{
    public interface IAccountService
    {
        /// <summary>根据账号名获取账号
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        Accounts.Model.Account GetAccount(string accountName);
    }
}
