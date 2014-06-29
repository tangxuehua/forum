namespace Forum.Domain.Accounts
{
    /// <summary>用于存储账号的唯一索引信息，记录了账号的名称和账号ID，账号名称有唯一性约束
    /// </summary>
    public interface IAccountIndexStore
    {
        AccountNameUniquenessValidateResult AddAccountNameIndex(string accountName, string accountId);
        string GetAccountId(string accountName);
    }
}
