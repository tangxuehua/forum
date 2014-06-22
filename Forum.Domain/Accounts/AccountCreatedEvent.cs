using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class AccountCreatedEvent : DomainEvent<string>
    {
        public AccountInfo AccountInfo { get; private set; }

        public AccountCreatedEvent(string accountId, AccountInfo accountInfo)
            : base(accountId)
        {
            AccountInfo = accountInfo;
        }
    }
}
