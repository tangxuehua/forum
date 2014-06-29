using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class AccountRejectedEvent : ProcessCompletedEvent<string>
    {
        public AccountRejectedEvent(string accountId, int reasonCode)
            : base(accountId)
        {
            IsSuccess = false;
            ErrorCode = reasonCode;
        }
    }
}
