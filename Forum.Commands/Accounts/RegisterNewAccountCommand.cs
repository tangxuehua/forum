using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class RegisterNewAccountCommand : AggregateCommand<string>, ICreatingAggregateCommand
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        private RegisterNewAccountCommand() { }
        public RegisterNewAccountCommand(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
