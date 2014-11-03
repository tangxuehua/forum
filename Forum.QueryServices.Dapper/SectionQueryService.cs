using System;
using System.Collections.Generic;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Infrastructure;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class SectionQueryService : BaseQueryService, ISectionQueryService
    {
        public IEnumerable<SectionInfo> FindAll()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<SectionInfo>(null, Constants.SectionTable);
            }
        }
        public dynamic FindDynamic(string id, string option)
        {
            if (option == "simple")
            {
                using (var connection = GetConnection())
                {
                    return connection.QueryList(new { Id = id }, Constants.SectionTable, "id,name").SingleOrDefault();
                }
            }
            throw new Exception("Invalid find option:" + option);
        }
    }
}
