using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public IEnumerable<PostInfo> QueryPosts(PostQueryOption option)
        {
            PageInfo.ValidateAndFixPageInfo(option.PageInfo);
            object condition = null;

            if (!string.IsNullOrEmpty(option.SectionId))
            {
                condition = new { SectionId = option.SectionId };
            }

            using (var connection = GetConnection())
            {
//                var sql = string.Format(@"
//                        select p.*, a.Name as AuthorName, count(r.*) as ReplyCount, max(r.Sequence) as MostRecentReplySequence from {0} p
//                        inner join {1} a on p.AuthorId = a.Id
//                        left join {2} r on r.PostId = p.Id
//                        group by p.Id",
//                    Constants.PostTable, Constants.AccountTable, Constants.ReplyTable);

                return connection.QueryPaged<PostInfo>(
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
                var sql = string.Format(@"
                        select p.*, a1.Name as AuthorName from {0} p inner join {1} a1 on p.AuthorId = a1.Id where p.Id = @PostId
                        select r.*, a2.Name as AuthorName from {2} r inner join {1} a2 on r.AuthorId = a2.Id where r.PostId = @PostId order by r.CreatedOn asc",
                        Constants.PostTable, Constants.AccountTable, Constants.ReplyTable);

                using (var multi = connection.QueryMultiple(sql, new { PostId = postId }))
                {
                    var postInfo = multi.Read<PostInfo>().SingleOrDefault();
                    if (postInfo != null)
                    {
                        var replyList = multi.Read<ReplyInfo>().ToList();
                        postInfo.ReplyList = replyList;
                        postInfo.ReplyCount = replyList.Count();
                        if (replyList.Count > 0)
                        {
                            var mostRecentReply = replyList.Last();
                            postInfo.MostRecentReplyId = mostRecentReply.Id;
                            postInfo.MostRecentReplierId = mostRecentReply.AuthorId;
                            postInfo.MostRecentReplierName = mostRecentReply.AuthorName;
                            postInfo.MostRecentReplyCreatedOn = mostRecentReply.CreatedOn;
                        }
                        return postInfo;
                    }
                    return null;
                }
            }
        }
    }
}
