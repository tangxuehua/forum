using System.Collections.Generic;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices
{
    /// <summary>版块查询服务
    /// </summary>
    public interface ISectionQueryService
    {
        /// <summary>查询所有版块
        /// </summary>
        /// <returns></returns>
        IEnumerable<SectionInfo> FindAll();
    }
}