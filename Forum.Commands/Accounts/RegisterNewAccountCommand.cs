using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class RegisterNewAccountCommand : Command
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        private RegisterNewAccountCommand() { }
        public RegisterNewAccountCommand(string id, string name, string password) : base(id)
        {
            Name = name;
            Password = password;
        }
    }
}
