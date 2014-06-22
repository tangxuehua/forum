using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class CancelRegistrationCommand : CancelProcessCommand<string>
    {
        public CancelRegistrationCommand(string processId, string registrationId, int errorCode)
            : base(processId, registrationId, errorCode)
        {
        }
    }
}
