namespace Forum.Domain.Accounts
{
    /// <summary>账号索引信息，用于支持账号名称的唯一性
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
