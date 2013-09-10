using System;
using ENode.Domain;
using ENode.Eventing;
using Forum.Events.Account;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class Account : AggregateRoot<Guid>, IEventHandler<AccountCreated>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public Account() { }
        public Account(string name, string password) : base(Guid.NewGuid())
        {
            Assert.IsNotNullOrWhiteSpace("name", name);
            Assert.IsNotNullOrEmpty("password", password);
            RaiseEvent(new AccountCreated(Id, name, password));
        }

        void IEventHandler<AccountCreated>.Handle(AccountCreated evnt)
        {
            Name = evnt.Name;
            Password = evnt.Password;
        }
    }
}
