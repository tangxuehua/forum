using System.Collections.Generic;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class PostQueryService : BaseQueryService, IPostQueryService
    {
        public PostQueryService(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public IEnumerable<Post> QueryPosts(PostQueryOption option)
        {
            PageInfo.ValidateAndFixPageInfo(option.PageInfo);
            object condition = null;

            if (option.SectionId != null)
            {
                condition = new { Section = option.SectionId.Value };
            }

            return ConnectionFactory.CreateConnection().TryExecute(connection => connection.QueryPaged<Post>(condition, "tb_Post", "*", "CreatedOn", option.PageInfo.PageIndex, option.PageInfo.PageSize));
        }
    }
}
