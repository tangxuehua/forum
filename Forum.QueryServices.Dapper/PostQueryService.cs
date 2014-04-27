using System.Collections.Generic;
using ECommon.Components;
using ECommon.Dapper;
using ECommon.Utilities;
using Forum.Infrastructure;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component(LifeStyle.Singleton)]
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
                    Constants.PostTable,
                    "CreatedOn",
                    option.PageInfo.PageIndex,
                    option.PageInfo.PageSize);
            }
        }
        public PostInfo QueryPost(string postId)
        {
            using (var connection = GetConnection())
            {
                var post = connection.QueryList<Post>(new { Id = postId }, Constants.PostTable);
                if (post != null)
                {
                    var replyList = connection.QueryList<ReplyInfo>(new { PostId = postId }, Constants.ReplyTable);
                    var postInfo = ObjectUtils.CreateObject<PostInfo>(post);
                    postInfo.ReplyList = replyList;
                    return postInfo;
                }
                return null;
            }
        }
    }
}
