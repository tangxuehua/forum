using System;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Infrastructure;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class ReplyQueryService : BaseQueryService, IReplyQueryService
    {
        public dynamic FindDynamic(string id, string option)
        {
            if (option == "simple")
            {
                using (var connection = GetConnection())
                {
                    return connection.QueryList(new { Id = id }, Constants.ReplyTable, "id,body,postId,parentId,authorId,createdOn").SingleOrDefault();
                }
            }
            throw new Exception("Invalid find option:" + option);
        }
    }
}
