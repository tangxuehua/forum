using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class RegistrationCompletedEvent : ProcessCompletedEvent<string>
    {
        public RegistrationCompletedEvent(string registrationId)
            : base(registrationId)
        {
        }
    }
}
