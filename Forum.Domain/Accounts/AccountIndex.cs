namespace Forum.Domain.Accounts
{
    /// <summary>账号索引聚合根，只包含账号ID和账号名称，用于支持账号名称的唯一性
    /// </summary>
    public class AccountIndex
    {
        public string AccountId { get; private set; }
        public string AccountName { get; private set; }

        public AccountIndex(string accountId, string accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }
    }
}
