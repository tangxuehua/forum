using System.Collections.Generic;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class PostQueryService : BaseQueryService, IPostQueryService
    {
        public PostQueryService(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public IEnumerable<Post> QueryPosts(PostQueryOption option)
        {
            return ConnectionFactory.CreateConnection().TryExecute(connection => connection.Query<Post>(new { Section = option.SectionId }, "tb_Post"));
        }
    }
}
