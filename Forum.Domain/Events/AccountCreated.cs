using System;
using ENode.Eventing;

namespace Forum.Domain.Events
{
    [Serializable]
    public class AccountCreated : Event
    {
        public Guid AccountId { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }

        public AccountCreated(Guid accountId, string name, string password)
        {
            AccountId = accountId;
            Name = name;
            Password = password;
        }
    }
}
