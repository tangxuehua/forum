using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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
        public IEnumerable<SectionAndStatistic> FindAllInculdeStatistic()
        {
            var sql = string.Format(@"select Id
            ,Name
            ,[Description]
            ,(select sum({0}.ReplyCount) from Post where {0}.SectionId={1}.Id) as ReplyCount
            ,(select count(0) from Post where {0}.SectionId={1}.Id) as PostCount 
            from {1}", Constants.PostTable, Constants.SectionTable);

            using (var connection = GetConnection())
            {
                return connection.Query<SectionAndStatistic>(sql);
            }
        }
        public dynamic FindDynamic(string id, string option)
        {
            var columns = GetColumns(option);

            using (var connection = GetConnection())
            {
                return connection.QueryList(new { Id = id }, Constants.SectionTable, columns).SingleOrDefault();
            }

        }
        public SectionAndStatistic FindInculdeStatisticById(string id)
        {
            var sql = string.Format(@"select Id
            ,Name
            ,[Description]
            ,(select sum({0}.ReplyCount) from Post where {0}.SectionId={1}.Id) as ReplyCount
            ,(select count(0) from Post where {0}.SectionId={1}.Id) as PostCount 
            from {1} where {1}.Id=@Id", Constants.PostTable, Constants.SectionTable);

            using (var connection = GetConnection())
            {
                return connection.Query<SectionAndStatistic>(sql, new { Id = id }).FirstOrDefault();
            }
        }

        private string GetColumns(string option)
        {
            string columns;
            switch (option)
            {
                case "simple":
                    columns = "Id,Name,Description";
                    break;
                case "detail":
                    columns = "Id,Name,Description,ReplyCount,PostCount";
                    break;
                default: throw new Exception("Invalid find option:" + option);
            }
            return columns;
        }
    }
}
