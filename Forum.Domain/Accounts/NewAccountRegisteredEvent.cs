using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class NewAccountRegisteredEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        private NewAccountRegisteredEvent() { }
        public NewAccountRegisteredEvent(string accountId, string name, string password)
            : base(accountId)
        {
            Name = name;
            Password = password;
        }
    }
}
