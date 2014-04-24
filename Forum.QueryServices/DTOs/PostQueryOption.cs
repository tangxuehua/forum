using System;

namespace Forum.QueryServices.DTOs
{
    /// <summary>帖子查询选项
    /// </summary>
    public class PostQueryOption
    {
        /// <summary>所属版块ID
        /// </summary>
        public string SectionId { get; set; }
        /// <summary>分页信息
        /// </summary>
        public PageInfo PageInfo { get; set; }

        public PostQueryOption()
        {
            PageInfo = PageInfo.Default;
        }
        public PostQueryOption(int pageIndex) : this()
        {
            PageInfo.PageIndex = pageIndex;
        }
    }
}
