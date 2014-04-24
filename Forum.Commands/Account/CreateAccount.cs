using System;
using ENode.Commanding;

namespace Forum.Commands.Account
{
    [Serializable]
    public class CreateAccount : Command<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public CreateAccount(string accountId, string name, string password) : base(accountId)
        {
            Name = name;
            Password = password;
        }
    }
}
