using System.Collections.Generic;
using Forum.Application.DTOs;

namespace Forum.Application.QueryServices
{
    /// <summary>主题列表查询服务
    /// </summary>
    public interface IThreadQueryService
    {
        /// <summary>查询主题列表
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        IEnumerable<Thread> QueryThreads(ThreadQueryOption option);
    }
}