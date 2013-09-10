using System;

namespace Forum.QueryServices.DTOs
{
    /// <summary>表示一个帖子以及该帖子的所有回复的统计信息
    /// </summary>
    public class Post
    {
        /// <summary>唯一标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>所属版块ID
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
