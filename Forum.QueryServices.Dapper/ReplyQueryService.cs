using System;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Infrastructure;

namespace Forum.QueryServices.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class ReplyQueryService : BaseQueryService, IReplyQueryService
    {
        public dynamic FindDynamic(string id, string option)
        {
            if (option == "simple")
            {
                using (var connection = GetConnection())
                {
                    return connection.QueryList(new { Id = id }, Constants.ReplyTable, "id,body,authorId").SingleOrDefault();
                }
            }
            throw new Exception("Invalid find option:" + option);
        }

        private static string FormatValue(object value)
        {
            if (value is DBNull || value == null)
            {
                return string.Empty;
            }
            return value.ToString();
        }
    }
}
