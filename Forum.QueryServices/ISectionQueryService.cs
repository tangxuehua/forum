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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SectionAndStatistic> FindAllInculdeStatistic();
        /// <summary>Find a single section, returns the dynamic data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        dynamic FindDynamic(string id, string option);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SectionAndStatistic FindInculdeStatisticById(string id);
    }
}