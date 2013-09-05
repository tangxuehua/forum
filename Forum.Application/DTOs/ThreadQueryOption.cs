using System;

namespace Forum.Application.DTOs
{
    /// <summary>表示主题的查询信息
    /// </summary>
    public class ThreadQueryOption
    {
        /// <summary>所属版块
        /// </summary>
        public Guid SectionId { get; set; }
    }
}
