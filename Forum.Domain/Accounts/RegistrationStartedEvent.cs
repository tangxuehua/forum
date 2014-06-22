using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class RegistrationStartedEvent : DomainEvent<string>
    {
        public AccountInfo AccountInfo { get; private set; }

        public RegistrationStartedEvent(string registrationId, AccountInfo accountInfo)
            : base(registrationId)
        {
            AccountInfo = accountInfo;
        }
    }
}
