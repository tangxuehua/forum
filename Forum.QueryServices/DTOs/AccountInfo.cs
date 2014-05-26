using System;

namespace Forum.QueryServices.DTOs
{
    /// <summary>表示一个账号信息
    /// </summary>
    public class AccountInfo
    {
        public AccountInfo()
        {
            Name = string.Empty;
        }

        /// <summary>账号ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>账号名
        /// </summary>
        public string Name { get; set; }
        /// <summary>账号密码
        /// </summary>
        public string Password { get; set; }
   }
}