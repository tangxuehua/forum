namespace Forum.Domain.Accounts
{
    /// <summary>账号注册信息仓储
    /// </summary>
    public interface IRegistrationRepository
    {
        /// <summary>添加一个账号注册信息
        /// </summary>
        /// <param name="registration"></param>
        void Add(Registration registration);
        /// <summary>更新一个账号注册信息
        /// </summary>
        /// <param name="registration"></param>
        void Update(Registration registration);
        /// <summary>根据账号名获取账号注册信息
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        Registration GetByAccountName(string accountName);
    }
}
