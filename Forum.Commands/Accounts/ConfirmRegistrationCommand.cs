using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class ConfirmRegistrationCommand : ProcessCommand<string>
    {
        public ConfirmRegistrationCommand(string processId, string registrationId)
            : base(processId, registrationId)
        {
        }
    }
}
