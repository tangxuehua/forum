using System;
using ENode.Eventing;

namespace Forum.Events.Account
{
    [Serializable]
    public class AccountCreated : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public AccountCreated(string accountId, string name, string password)
            : base(accountId)
        {
            Name = name;
            Password = password;
        }
    }
}
