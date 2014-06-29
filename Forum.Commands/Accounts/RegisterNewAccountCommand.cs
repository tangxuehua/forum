using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class RegisterNewAccountCommand : ProcessCommand<string>, ICreatingAggregateCommand
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public RegisterNewAccountCommand(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
