using System;
using ENode.Domain;
using Forum.Infrastructure;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class Account : AggregateRoot<string>
    {
        public AccountInfo AccountInfo { get; private set; }
        public AccountStatus Status { get; private set; }

        public Account(string id, AccountInfo accountInfo)
            : base(id)
        {
            Assert.IsNotNull("账号信息", accountInfo);
            Assert.IsNotNullOrWhiteSpace("用户名", accountInfo.Name);
            Assert.IsNotNullOrWhiteSpace("密码", accountInfo.Password);
            RaiseEvent(new NewAccountRegisteredEvent(Id, accountInfo));
        }

        public void Confirm()
        {
            if (Status == AccountStatus.NewRegistered)
            {
                RaiseEvent(new AccountConfirmedEvent(Id, AccountInfo));
            }
        }
        public void Reject(int reasonCode)
        {
            if (Status == AccountStatus.NewRegistered)
            {
                RaiseEvent(new AccountRejectedEvent(Id, reasonCode));
            }
        }

        private void Handle(NewAccountRegisteredEvent evnt)
        {
            Id = evnt.AggregateRootId;
            AccountInfo = evnt.AccountInfo;
            Status = AccountStatus.NewRegistered;
        }
        private void Handle(AccountConfirmedEvent evnt)
        {
            Status = AccountStatus.Confirmed;
        }
        private void Handle(AccountRejectedEvent evnt)
        {
            Status = AccountStatus.Rejected;
        }
    }

    /// <summary>账号信息，注册时用户录入的信息，值对象
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
    /// <summary>账号状态
    /// </summary>
    public enum AccountStatus
    {
        NewRegistered = 1,
        Confirmed,
        Rejected,
    }
}
