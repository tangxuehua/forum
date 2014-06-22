using System;
using ENode.Domain;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class Account : AggregateRoot<string>
    {
        public AccountInfo AccountInfo { get; private set; }

        public Account(string id, AccountInfo accountInfo)
            : base(id)
        {
            Assert.IsNotNull("账号信息", accountInfo);
            Assert.IsNotNullOrWhiteSpace("用户名", accountInfo.Name);
            Assert.IsNotNullOrWhiteSpace("密码", accountInfo.Password);
            RaiseEvent(new AccountCreatedEvent(Id, accountInfo));
        }

        private void Handle(AccountCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            AccountInfo = evnt.AccountInfo;
        }
    }

    /// <summary>账号基本信息，注册时用户录入的信息，值对象
    /// </summary>
    [Serializable]
    public class AccountInfo
    {
        public string Name { get; private set; }
        public string Password { get; private set; }

        public AccountInfo(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
