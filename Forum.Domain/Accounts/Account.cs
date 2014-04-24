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
            Assert.IsNotNullOrWhiteSpace("用户名", name);
            Assert.IsNotNullOrEmpty("密码", password);
            RaiseEvent(new AccountCreatedEvent(Id, name, password));
        }

        private void Handle(AccountCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            Name = evnt.Name;
            Password = evnt.Password;
        }
    }
}
