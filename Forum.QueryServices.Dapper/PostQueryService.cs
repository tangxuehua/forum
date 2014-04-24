using System;
using System.Collections.Generic;
using ECommon.IoC;
using ECommon.Utilities;
using ENode.Infrastructure.Dapper;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class PostQueryService : BaseQueryService, IPostQueryService
    {
        public IEnumerable<Post> QueryPosts(PostQueryOption option)
        {
            PageInfo.ValidateAndFixPageInfo(option.PageInfo);
            object condition = null;

            if (!string.IsNullOrEmpty(option.SectionId))
            {
                condition = new { SectionId = option.SectionId };
            }

            using (var connection = GetConnection())
            {
                return connection.QueryPaged<Post>(
                    condition,
                    "tb_Post",
                    "*",
                    "CreatedOn",
                    option.PageInfo.PageIndex - 1,
                    option.PageInfo.PageSize);
            }
        }
        public PostInfo QueryPost(string postId)
        {
            using (var connection = GetConnection())
            {
                var post = connection.QuerySingleOrDefault<Post>(new { Id = postId }, "tb_Post");
                if (post != null)
                {
                    var replyList = connection.Query<ReplyInfo>(new { PostId = postId }, "tb_Reply");
                    var postInfo = ObjectUtils.CreateObject<PostInfo>(post);
                    postInfo.ReplyList = replyList;
                    return postInfo;
                }
                return null;
            }
        }
    }
}
