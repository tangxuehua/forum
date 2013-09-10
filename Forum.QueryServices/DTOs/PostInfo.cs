using System;
using System.Collections.Generic;

namespace Forum.QueryServices.DTOs
{
    /// <summary>表示一个帖子的详情信息
    /// </summary>
    public class PostInfo
    {
        /// <summary>唯一标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>所属版块ID
        /// </summary>
        public Guid SectionId { get; set; }
        /// <summary>作者ID
        /// </summary>
        public Guid AuthorId { get; set; }
        /// <summary>作者名称
        /// </summary>
        public string AuthorName { get; set; }
        /// <summary>创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>回复列表
        /// </summary>
        public IEnumerable<ReplyInfo> ReplyList { get; set; }
    }
}
