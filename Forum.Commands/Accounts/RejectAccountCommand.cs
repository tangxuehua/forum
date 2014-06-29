using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class RejectAccountCommand : ProcessCommand<string>
    {
        public int ReasonCode { get; private set; }

        public RejectAccountCommand(string accountId, int reasonCode)
            : base(accountId)
        {
            ReasonCode = reasonCode;
        }
    }
}
