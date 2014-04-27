using System;
using ENode.Infrastructure;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class DuplicateAccountNameException : ENodeException
    {
        public DuplicateAccountNameException(string accountName, Exception innerException) : base(string.Format("用户账号重复，账号：{0}", accountName), innerException) { }
    }
}
