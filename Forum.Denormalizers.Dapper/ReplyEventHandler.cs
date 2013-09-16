using System;
using System.Data;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.Events.Reply;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class ReplyEventHandler : BaseEventHandler,
        IEventHandler<PostReplied>,
        IEventHandler<ReplyReplied>,
        IEventHandler<ReplyBodyChanged>
    {
        public ReplyEventHandler(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void Handle(PostReplied evnt)
        {
            ConnectionFactory.CreateConnection().TryExecuteInTransaction((connection, transaction) => ProcessRepliedEvent(connection, transaction, evnt.Info, null));
        }
        public void Handle(ReplyReplied evnt)
        {
            ConnectionFactory.CreateConnection().TryExecuteInTransaction((connection, transaction) => ProcessRepliedEvent(connection, transaction, evnt.Info, evnt.ParentReplyId));
        }
        public void Handle(ReplyBodyChanged evnt)
        {
            ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                connection.Update(new { Body = evnt.Body }, new { Id = evnt.ReplyId }, "tb_Reply");
            });
        }

        private static void ProcessRepliedEvent(IDbConnection connection, IDbTransaction transaction, RepliedEventInfo info, Guid? parentReplyId)
        {
            var replyCount = GetReplyCount(connection, transaction, info.PostId);
            var authorName = GetAuthorName(connection, transaction, info.AuthorId);
            UpdatePostStatisticInfo(connection, transaction, info.PostId, replyCount + 1, info.AuthorId, authorName, info.CreatedOn);
            CreateReply(connection, transaction, info.ReplyId, parentReplyId, info.PostId, info.Body, info.AuthorId, authorName, replyCount + 1, info.CreatedOn);
        }
        private static int GetReplyCount(IDbConnection connection, IDbTransaction transaction, Guid postId)
        {
            return connection.GetValue<int>(new {Id = postId}, "tb_Post", "ReplyCount", transaction);
        }
        private static string GetAuthorName(IDbConnection connection, IDbTransaction transaction, Guid authorId)
        {
            return connection.GetValue<string>(new { Id = authorId }, "tb_Account", "Name", transaction);
        }
        private static void CreateReply(IDbConnection connection, IDbTransaction transaction, Guid id, Guid? parentId, Guid postId, string body, Guid authorId, string authorName, int floorIndex, DateTime createdOn)
        {
            connection.Insert(
                new
                {
                    Id = id,
                    ParentId = parentId,
                    PostId = postId,
                    Body = body,
                    AuthorId = authorId,
                    AuthorName = authorName,
                    FloorIndex = floorIndex,
                    CreatedOn = createdOn
                },
                "tb_Reply", transaction);
        }
        private static void UpdatePostStatisticInfo(IDbConnection connection, IDbTransaction transaction, Guid postId, int replyCount, Guid mostRecentReplierId, string mostRecentReplierName, DateTime mostRecentReplyCreatedOn)
        {
            connection.Update(
                new
                {
                    ReplyCount = replyCount,
                    MostRecentReplierId = mostRecentReplierId,
                    MostRecentReplierName = mostRecentReplierName,
                    MostRecentReplyCreatedOn = mostRecentReplyCreatedOn
                },
                new
                {
                    Id = postId
                },
                "tb_Post", transaction);
        }
    }
}
