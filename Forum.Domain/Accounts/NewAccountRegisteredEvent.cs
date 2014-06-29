using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class NewAccountRegisteredEvent : DomainEvent<string>
    {
        public AccountInfo AccountInfo { get; private set; }

        public NewAccountRegisteredEvent(string accountId, AccountInfo accountInfo)
            : base(accountId)
        {
            AccountInfo = accountInfo;
        }
    }
}
