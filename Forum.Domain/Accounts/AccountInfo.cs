using System;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
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
