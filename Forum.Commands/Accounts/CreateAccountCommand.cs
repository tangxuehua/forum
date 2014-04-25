using System;
using ENode.Commanding;

namespace Forum.Commands.Accounts
{
    [Serializable]
    public class CreateAccountCommand : Command<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public CreateAccountCommand(string id, string name, string password) : base(id)
        {
            Name = name;
            Password = password;
        }
    }
}
