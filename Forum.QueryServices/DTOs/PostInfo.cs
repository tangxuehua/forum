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
        public string Id { get; set; }
        /// <summary>标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>所属版块ID
        /// </summary>
        public string SectionId { get; set; }
        /// <summary>作者ID
        /// </summary>
        public string AuthorId { get; set; }
        /// <summary>作者名称
        /// </summary>
        public string AuthorName { get; set; }
        /// <summary>创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        /// <summary>回复列表
        /// </summary>
        public IEnumerable<ReplyInfo> ReplyList { get; set; }
        /// <summary>总回复数
        /// </summary>
        public int ReplyCount { get; set; }
        /// <summary>最近一个回复ID
        /// </summary>
        public string MostRecentReplyId { get; set; }
        /// <summary>最近一个回复的作者ID
        /// </summary>
        public string MostRecentReplierId { get; set; }
        /// <summary>最近一个回复的作者名称
        /// </summary>
        public string MostRecentReplierName { get; set; }
        /// <summary>最近一个回复的创建时间
        /// </summary>
        public DateTime MostRecentReplyCreatedOn { get; set; }
    }
}
