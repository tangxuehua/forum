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
            ConnectionFactory.CreateConnection().TryExecuteInTransaction((connection, transaction) =>
            {
                var replyCount = connection.GetValue<int>(new { Id = evnt.PostId }, "tb_Post", "ReplyCount", transaction);
                CreateReply(connection, transaction, evnt.ReplyId, null, evnt.PostId, evnt.Body, evnt.AuthorId, replyCount + 1, evnt.CreatedOn);
                var authorName = connection.GetValue<string>(new { Id = evnt.AuthorId }, "tb_Account", "Name", transaction);
                UpdatePostStatisticInfo(connection, transaction, evnt.PostId, replyCount + 1, evnt.AuthorId, authorName, evnt.CreatedOn);
            });
        }
        public void Handle(ReplyReplied evnt)
        {
            ConnectionFactory.CreateConnection().TryExecuteInTransaction((connection, transaction) =>
            {
                var replyCount = connection.GetValue<int>(new { Id = evnt.PostId }, "tb_Post", "ReplyCount", transaction);
                CreateReply(connection, transaction, evnt.ReplyId, evnt.ParentReplyId, evnt.PostId, evnt.Body, evnt.AuthorId, replyCount + 1, evnt.CreatedOn);
                var authorName = connection.GetValue<string>(new { Id = evnt.AuthorId }, "tb_Account", "Name", transaction);
                UpdatePostStatisticInfo(connection, transaction, evnt.PostId, replyCount + 1, evnt.AuthorId, authorName, evnt.CreatedOn);
            });
        }
        public void Handle(ReplyBodyChanged evnt)
        {
            ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                connection.Update(new { Body = evnt.Body }, new { Id = evnt.ReplyId }, "tb_Reply");
            });
        }

        private static void CreateReply(IDbConnection connection, IDbTransaction transaction, Guid id, Guid? parentId, Guid postId, string body, Guid authorId, int floorIndex, DateTime createdOn)
        {
            connection.Insert(
                new
                {
                    Id = id,
                    ParentId = parentId,
                    PostId = postId,
                    Body = body,
                    AuthorId = authorId,
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
