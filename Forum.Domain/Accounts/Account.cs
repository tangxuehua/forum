using System;
using ENode.Domain;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    /// <summary>账号聚合根
    /// </summary>
    [Serializable]
    public class Account : AggregateRoot<string>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public Account(string id, string name, string password)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("用户名", name);
            Assert.IsNotNullOrWhiteSpace("密码", password);
            RaiseEvent(new NewAccountRegisteredEvent(Id, name, password));
        }

        private void Handle(NewAccountRegisteredEvent evnt)
        {
            Id = evnt.AggregateRootId;
            Name = evnt.Name;
            Password = evnt.Password;
        }
    }
}
