using System.Collections.Generic;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices
{
    public interface ISectionQueryService
    {
        /// <summary>Find all the sections.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SectionInfo> FindAll();
    }
}