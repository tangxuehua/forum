using System;

namespace Forum.Domain.Accounts
{
    /// <summary>账号注册信息
    /// </summary>
    [Serializable]
    public class Registration
    {
        /// <summary>账号ID
        /// </summary>
        public string AccountId { get; private set; }
        /// <summary>账号名
        /// </summary>
        public string AccountName { get; private set; }
        /// <summary>账号注册状态
        /// </summary>
        public RegistrationStatus RegistrationStatus { get; private set; }

        public Registration(string accountId, string accountName) : this(accountId, accountName, RegistrationStatus.Created) { }
        public Registration(string accountId, string accountName, RegistrationStatus registrationStatus)
        {
            AccountId = accountId;
            AccountName = accountName;
            RegistrationStatus = registrationStatus;
        }

        /// <summary>确认账号注册信息，状态修改为已确认
        /// </summary>
        public void ConfirmStatus()
        {
            RegistrationStatus = RegistrationStatus.Confirmed;
        }
    }
}