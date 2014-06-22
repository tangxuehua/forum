using System;
using ENode.Eventing;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class RegistrationCanceledEvent : ProcessCompletedEvent<string>
    {
        public RegistrationCanceledEvent(string registrationId, int errorCode)
            : base(registrationId)
        {
            IsSuccess = false;
            ErrorCode = errorCode;
        }
    }
}
