using System.Collections.Generic;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices
{
    /// <summary>帖子查询服务
    /// </summary>
    public interface IPostQueryService
    {
        /// <summary>查询帖子列表
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        IEnumerable<PostInfo> QueryPosts(PostQueryOption option);
        /// <summary>查询帖子详情
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        PostInfo QueryPost(string postId);
    }
}