namespace Forum.Domain.Accounts
{
    /// <summary>账号索引信息的仓储接口，用于存储账号的唯一索引信息，实现账号名称的唯一性约束
    /// </summary>
    public interface IAccountIndexRepository
    {
        AccountIndex FindByIndexId(string indexId);
        AccountIndex FindByAccountName(string accountName);
        void Add(AccountIndex index);
    }
}
