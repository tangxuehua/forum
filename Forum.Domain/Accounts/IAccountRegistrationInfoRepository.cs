namespace Forum.Domain.Accounts
{
    /// <summary>账号注册信息仓储
    /// </summary>
    public interface IAccountRegistrationInfoRepository
    {
        /// <summary>添加一个账号注册信息
        /// </summary>
        /// <param name="info"></param>
        void Add(AccountRegistrationInfo info);
        /// <summary>更新一个账号注册信息
        /// </summary>
        /// <param name="info"></param>
        void Update(AccountRegistrationInfo info);
        /// <summary>根据账号名获取账号注册信息
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        AccountRegistrationInfo GetByAccountName(string accountName);
    }
}
