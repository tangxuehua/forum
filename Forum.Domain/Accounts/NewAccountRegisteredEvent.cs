using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Accounts
{
    [Code(10)]
    public class NewAccountRegisteredEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        private NewAccountRegisteredEvent() { }
        public NewAccountRegisteredEvent(Account account, string name, string password)
            : base(account)
        {
            Name = name;
            Password = password;
        }
    }
}
