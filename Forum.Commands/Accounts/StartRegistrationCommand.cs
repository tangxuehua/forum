using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class StartRegistrationCommand : ProcessCommand<string>, ICreatingAggregateCommand
    {
        public string AccountName { get; private set; }
        public string AccountPassword { get; private set; }

        public StartRegistrationCommand(string accountName, string accountPasword)
        {
            AccountName = accountName;
            AccountPassword = accountPasword;
        }
    }
}
