using System;
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

            return ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                return connection.QueryPaged<Post>(
                    condition,
                    "tb_Post",
                    "*",
                    "CreatedOn",
                    option.PageInfo.PageIndex,
                    option.PageInfo.PageSize);
            });
        }
        public PostInfo QueryPost(Guid postId)
        {
            return ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                var post = connection.QuerySingleOrDefault<Post>(new { Id = postId }, "tb_Post");
                if (post != null)
                {
                    var replyList = connection.Query<ReplyInfo>(new { PostId = postId }, "tb_Reply");
                    var postInfo = Utils.CreateObject<PostInfo>(post);
                    postInfo.ReplyList = replyList;
                    return postInfo;
                }
                return null;
            });
        }
    }
}
