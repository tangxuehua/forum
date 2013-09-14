using System;

namespace Forum.QueryServices.DTOs
{
    /// <summary>分页信息
    /// </summary>
    public class PageInfo
    {
        /// <summary>默认的分页信息
        /// </summary>
        public static PageInfo Default = new PageInfo { PageIndex = 1, PageSize = 20 };

        /// <summary>当前页（从1开始）
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>验证给定的分页信息，如果不合法，则返回一个合法的分页信息
        /// </summary>
        /// <param name="pageInfo"></param>
        public static void ValidateAndFixPageInfo(PageInfo pageInfo)
        {
            if (pageInfo == null)
            {
                pageInfo = PageInfo.Default;
            }
            if (pageInfo.PageIndex < 1)
            {
                pageInfo.PageIndex = 1;
            }
            if (pageInfo.PageSize < 1)
            {
                pageInfo.PageSize = 20;
            }
        }
    }
}
