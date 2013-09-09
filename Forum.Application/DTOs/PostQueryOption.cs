using System;

namespace Forum.Application.DTOs
{
    /// <summary>帖子查询选项
    /// </summary>
    public class PostQueryOption
    {
        /// <summary>所属版块ID
        /// </summary>
        public Guid SectionId { get; set; }
    }
}
