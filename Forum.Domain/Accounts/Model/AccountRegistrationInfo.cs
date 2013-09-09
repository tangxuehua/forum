using System;

namespace Forum.Domain.Accounts.Model
{
    /// <summary>账号注册信息
    /// </summary>
    [Serializable]
    public class AccountRegistrationInfo
    {
        /// <summary>账号ID
        /// </summary>
        public Guid AccountId { get; private set; }
        /// <summary>账号名
        /// </summary>
        public string AccountName { get; private set; }
        /// <summary>账号注册状态
        /// </summary>
        public AccountRegistrationStatus RegistrationStatus { get; private set; }

        public AccountRegistrationInfo(Guid accountId, string accountName)
            : this(accountId, accountName, AccountRegistrationStatus.Created) { }
        public AccountRegistrationInfo(Guid accountId, string accountName, AccountRegistrationStatus registrationStatus)
        {
            AccountId = accountId;
            AccountName = accountName;
            RegistrationStatus = registrationStatus;
        }

        /// <summary>确认账号注册信息，状态修改为已确认
        /// </summary>
        public void ConfirmStatus()
        {
            RegistrationStatus = AccountRegistrationStatus.Confirmed;
        }
    }
}