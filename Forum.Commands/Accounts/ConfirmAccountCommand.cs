using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class ConfirmAccountCommand : ProcessCommand<string>
    {
        public ConfirmAccountCommand(string accountId)
            : base(accountId)
        {
        }
    }
}
