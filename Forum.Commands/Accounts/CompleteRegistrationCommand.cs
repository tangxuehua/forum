using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class CompleteRegistrationCommand : ProcessCommand<string>
    {
        public CompleteRegistrationCommand(string processId, string registrationId)
            : base(processId, registrationId)
        {
        }
    }
}
