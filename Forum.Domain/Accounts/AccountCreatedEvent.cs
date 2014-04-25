using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class AccountCreatedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public AccountCreatedEvent(string accountId, string name, string password) : base(accountId)
        {
            Name = name;
            Password = password;
        }
    }
}
