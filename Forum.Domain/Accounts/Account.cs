using System;
using ENode.Domain;
using ENode.Infrastructure;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    /// <summary>账号聚合根
    /// </summary>
    public class Account : AggregateRoot<string>
    {
        private string _name;
        private string _password;

        public string Name
        {
            get { return _name; }
        }
        public string Password
        {
            get { return _password; }
        }

        public Account(string id, string name, string password)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("账号", name);
            Assert.IsNotNullOrWhiteSpace("密码", password);
            if (name.Length > 128)
            {
                throw new Exception("账号长度不能超过128");
            }
            if (password.Length > 128)
            {
                throw new Exception("密码长度不能超过128");
            }
            ApplyEvent(new NewAccountRegisteredEvent( name, password));
        }

        private void Handle(NewAccountRegisteredEvent evnt)
        {
            _name = evnt.Name;
            _password = evnt.Password;
        }
    }
}