using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Accounts
{
    public class NewAccountRegisteredEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        private NewAccountRegisteredEvent() { }
        public NewAccountRegisteredEvent(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
