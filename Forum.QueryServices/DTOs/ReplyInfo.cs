using System;

namespace Forum.QueryServices.DTOs
{
    /// <summary>表示一个回复信息
    /// </summary>
    public class ReplyInfo
    {
        public ReplyInfo()
        {
            AuthorName = string.Empty;
        }
        /// <summary>唯一标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>作者ID
        /// </summary>
        public string AuthorId { get; set; }
        /// <summary>作者名称
        /// </summary>
        public string AuthorName { get; set; }
        /// <summary>创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>楼层，即第几个回复，从1开始；
        /// </summary>
        public int Floor { get; set; }
    }
}
