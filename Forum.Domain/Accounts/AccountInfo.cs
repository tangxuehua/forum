using System;

namespace Forum.Domain.Accounts
{
    public class AccountInfo
    {
        public string AccountId { get; private set; }
        public string AccountName { get; private set; }

        public AccountInfo(string accountId, string accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }
    }
}