using System;

namespace Forum.Repository {
    [Serializable]
    public class DuplicateAccountNameException : Exception {
        public DuplicateAccountNameException(string accountName, Exception innerException) : base(string.Format("用户账号重复，账号：{0}", accountName), innerException) { }
    }
}
