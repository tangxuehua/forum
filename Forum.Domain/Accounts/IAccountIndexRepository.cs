namespace Forum.Domain.Accounts
{
    /// <summary>账号索引信息的仓储接口，用于存储账号的唯一索引信息，实现账号名称的唯一性约束
    /// </summary>
    public interface IAccountIndexRepository
    {
        /// <summary>根据账号名称检索账号索引信息
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        AccountIndex FindByAccountName(string accountName);
        /// <summary>添加一个账号索引
        /// </summary>
        /// <param name="index"></param>
        void Add(AccountIndex index);
    }
}
