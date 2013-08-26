using System;
using System.Collections.Generic;

namespace Forum.Application.QueryServices
{
    /// <summary>帖子列表查询服务
    /// </summary>
    public interface IThreadQueryService
    {
        /// <summary>根据板块ID查询帖子列表
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryThreads(Guid sectionId);
    }
}