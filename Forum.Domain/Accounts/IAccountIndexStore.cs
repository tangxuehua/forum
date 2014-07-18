namespace Forum.Domain.Accounts
{
    /// <summary>对账号索引信息的抽象接口，用于存储账号的唯一索引信息，实现账号名称的唯一性约束
    /// </summary>
    public interface IAccountIndexStore
    {
        AccountIndex FindByAccountName(string accountName);
        void Add(AccountIndex index);
    }
}
