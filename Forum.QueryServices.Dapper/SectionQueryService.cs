using System.Collections.Generic;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Infrastructure;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class SectionQueryService : BaseQueryService, ISectionQueryService
    {
        public IEnumerable<SectionInfo> FindAll()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<SectionInfo>(null, Constants.SectionTable);
            }
        }
    }
}
