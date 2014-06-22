using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class RegistrationConfirmedEvent : DomainEvent<string>
    {
        public RegistrationConfirmedEvent(string registrationId)
            : base(registrationId)
        {
        }
    }
}
