namespace Forum.Domain.Model.Account
{
    /// <summary>账号注册状态
    /// </summary>
    public enum AccountRegistrationStatus
    {
        /// <summary>账号已创建，但还未被确认
        /// </summary>
        Created = 1,
        /// <summary>账号已创建且已被确认
        /// </summary>
        Confirmed = 2
    }
}