using System;
using ENode.Domain;
using Forum.Events.Account;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class Account : AggregateRoot<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public Account(string id, string name, string password) : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("name", name);
            Assert.IsNotNullOrEmpty("password", password);
            RaiseEvent(new AccountCreated(Id, name, password));
        }

        private void Handle(AccountCreated evnt)
        {
            Id = evnt.AggregateRootId;
            Name = evnt.Name;
            Password = evnt.Password;
        }
    }
}
