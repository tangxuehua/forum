using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class AccountConfirmedEvent : ProcessCompletedEvent<string>
    {
        public AccountInfo AccountInfo { get; private set; }

        public AccountConfirmedEvent(string accountId, AccountInfo accountInfo)
            : base(accountId)
        {
            AccountInfo = accountInfo;
        }
    }
}
