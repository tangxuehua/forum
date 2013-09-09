using System.Collections.Generic;
using Forum.Application.DTOs;

namespace Forum.Application.QueryServices
{
    /// <summary>帖子查询服务
    /// </summary>
    public interface IPostQueryService
    {
        /// <summary>查询帖子列表
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        IEnumerable<Post> QueryPosts(PostQueryOption option);
    }
}