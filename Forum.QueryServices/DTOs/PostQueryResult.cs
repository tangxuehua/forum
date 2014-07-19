using System.Collections.Generic;

namespace Forum.QueryServices.DTOs
{
    /// <summary>分页查询帖子的结果，包含总页数
    /// </summary>
    public class PostQueryResult
    {
        public PostQueryResult()
        {
            Posts = new PostInfo[0];
        }
        /// <summary>当前页的帖子
        /// </summary>
        public IEnumerable<PostInfo> Posts { get; set; }
        /// <summary>总帖子数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
