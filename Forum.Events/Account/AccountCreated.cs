using System;
using ENode.Eventing;

namespace Forum.Events.Account
{
    [Serializable]
    public class AccountCreated : Event
    {
        public Guid AccountId { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }

        public AccountCreated(Guid accountId, string name, string password) : base(accountId)
        {
            AccountId = accountId;
            Name = name;
            Password = password;
        }
    }
}
