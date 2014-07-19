using System;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Infrastructure;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class PostQueryService : BaseQueryService, IPostQueryService
    {
        public PostQueryResult Find(PostQueryOption option)
        {
            object condition = null;
            var wherePart = string.Empty;

            if (!string.IsNullOrEmpty(option.SectionId) && !string.IsNullOrEmpty(option.AuthorId))
            {
                condition = new { SectionId = option.SectionId, AuthorId = option.AuthorId };
                wherePart = "WHERE p.SectionId = @SectionId and p.AuthorId = @AuthorId";
            }
            else if (!string.IsNullOrEmpty(option.SectionId))
            {
                condition = new { SectionId = option.SectionId };
                wherePart = "WHERE p.SectionId = @SectionId";
            }
            else if (!string.IsNullOrEmpty(option.AuthorId))
            {
                condition = new { AuthorId = option.AuthorId };
                wherePart = "WHERE p.AuthorId = @AuthorId";
            }

            using (var connection = GetConnection())
            {
                var countSql = string.Format(@"SELECT COUNT(1) FROM {0} p {1}", Constants.PostTable, wherePart);
                var totalCount = connection.Query<int>(countSql, condition).Single();

                var pageIndex = option.PageInfo.PageIndex;
                var pageSize = option.PageInfo.PageSize;
                var sql = string.Format(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY p.Sequence) AS RowNumber, p.*, a.Name as AuthorName, r.ReplyCount, r.MostRecentReplySequence
                            FROM {0} p
                            LEFT JOIN {1} a ON p.AuthorId = a.Id
                            LEFT JOIN (SELECT PostId, COUNT(*) AS ReplyCount, MAX(Sequence) AS MostRecentReplySequence FROM {2} GROUP BY PostId) r on r.PostId = p.Id
                            {3}) AS Total
                        WHERE RowNumber >= {4} AND RowNumber <= {5}",
                    Constants.PostTable, Constants.AccountTable, Constants.ReplyTable, wherePart, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

                var posts = connection.Query<PostInfo>(sql, condition);

                var sequenceIds = string.Join(",", posts.Select(x => x.MostRecentReplySequence));
                if (sequenceIds.Count() > 0)
                {
                    sql = string.Format(@"
                        select r.Id, r.Sequence, r.AuthorId, a.Name as AuthorName, r.CreatedOn from {0} r left join {1} a on r.AuthorId = a.Id where r.Sequence in ({2})",
                            Constants.ReplyTable, Constants.AccountTable, sequenceIds);
                    var replies = connection.Query(sql);
                    foreach (var post in posts)
                    {
                        post.AuthorName = FormatValue(post.AuthorName);
                        var mostRecentReply = replies.SingleOrDefault(x => x.Sequence == post.MostRecentReplySequence);
                        if (mostRecentReply != null)
                        {
                            post.MostRecentReplyId = FormatValue(mostRecentReply.Id);
                            post.MostRecentReplierId = FormatValue(mostRecentReply.AuthorId);
                            post.MostRecentReplierName = FormatValue(mostRecentReply.AuthorName);
                            post.MostRecentReplyCreatedOn = mostRecentReply.CreatedOn;
                        }
                    }
                }

                return new PostQueryResult { Posts = posts, TotalCount = totalCount };
            }
        }
        public PostInfo Find(string postId)
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format(@"
                        select p.*, a1.Name as AuthorName from {0} p left join {1} a1 on p.AuthorId = a1.Id where p.Id = @PostId
                        select r.*, a2.Name as AuthorName from {2} r left join {1} a2 on r.AuthorId = a2.Id where r.PostId = @PostId order by r.Sequence asc",
                        Constants.PostTable, Constants.AccountTable, Constants.ReplyTable);

                using (var multi = connection.QueryMultiple(sql, new { PostId = postId }))
                {
                    var post = multi.Read<PostInfo>().SingleOrDefault();
                    if (post != null)
                    {
                        var replyList = multi.Read<ReplyInfo>().ToList();
                        for (var index = 0; index < replyList.Count; index++)
                        {
                            replyList[index].Floor = index + 1;
                        }
                        post.ReplyList = replyList;
                        post.ReplyCount = replyList.Count();
                        if (replyList.Count > 0)
                        {
                            var mostRecentReply = replyList.Last();
                            post.MostRecentReplyId = FormatValue(mostRecentReply.Id);
                            post.MostRecentReplierId = FormatValue(mostRecentReply.AuthorId);
                            post.MostRecentReplierName = FormatValue(mostRecentReply.AuthorName);
                            post.MostRecentReplyCreatedOn = mostRecentReply.CreatedOn;
                        }

                        return post;
                    }
                    return null;
                }
            }
        }
        public dynamic FindDynamic(string id, string option)
        {
            if (option == "simple")
            {
                using (var connection = GetConnection())
                {
                    return connection.QueryList(new { Id = id }, Constants.PostTable, "id,subject,body,authorId").SingleOrDefault();
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
