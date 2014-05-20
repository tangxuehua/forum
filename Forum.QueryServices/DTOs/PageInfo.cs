using System;

namespace Forum.QueryServices.DTOs
{
    /// <summary>分页信息
    /// </summary>
    public class PageInfo
    {
        /// <summary>当前页（从1开始）
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>每页大小
        /// </summary>
        public int PageSize { get; set; }

        public PageInfo(int pageIndex = 1, int pageSize = 20)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("pageIndex cannot small than 1.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException("pageSize cannot small than 1.");
            }
            PageIndex = 1;
            PageSize = 20;
        }
    }
}
