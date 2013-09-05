using System;

namespace Forum.Application.DTOs
{
    /// <summary>表示一个主题，一个主题是对一个根帖子及其所有回复帖子的统计信息。
    /// </summary>
    public class Thread
    {
        /// <summary>唯一标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>所属版块
        /// </summary>
        public Guid SectionId { get; set; }
        /// <summary>作者ID
        /// </summary>
        public Guid AuthorId { get; set; }
        /// <summary>作者名称
        /// </summary>
        public string AuthorName { get; set; }
        /// <summary>总回复数
        /// </summary>
        public int ReplyCount { get; set; }
        /// <summary>创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>最近一个回复的作者ID
        /// </summary>
        public Guid MostRecentReplierId { get; set; }
        /// <summary>最近一个回复的作者名称
        /// </summary>
        public string MostRecentReplierName { get; set; }
        /// <summary>最近一个回复的创建时间
        /// </summary>
        public DateTime MostRecentReplyCreatedOn { get; set; }
    }
}
