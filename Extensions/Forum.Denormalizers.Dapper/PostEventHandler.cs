using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.Domain.Events;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class PostEventHandler : BaseEventHandler,
        IEventHandler<PostCreated>,
        IEventHandler<PostBodyChanged>
    {
        public PostEventHandler(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void Handle(PostCreated evnt)
        {
            ConnectionFactory.CreateConnection().TryExecuteInTransaction((connection, transaction) =>
            {
                if (evnt.ParentId == null)
                {
                    connection.Insert(
                        new
                        {
                            Id = evnt.PostId,
                            RootId = evnt.RootId,
                            Subject = evnt.Subject,
                            Body = evnt.Body,
                            SectionId = evnt.SectionId,
                            AuthorId = evnt.AuthorId,
                            FloorIndex = 0,
                            CreatedOn = evnt.CreatedOn
                        },
                        "tb_Post", transaction);
                    var authorName = connection.GetValue<string>(new { Id = evnt.AuthorId }, "tb_Account", "Name", transaction);
                    connection.Insert(
                        new
                        {
                            Id = evnt.PostId,
                            Subject = evnt.Subject,
                            AuthorId = evnt.AuthorId,
                            AuthorName = authorName,
                            SectionId = evnt.SectionId,
                            ReplyCount = 0,
                            CreatedOn = evnt.CreatedOn
                        },
                        "tb_Thread", transaction);
                }
                else
                {
                    var replyCount = connection.GetValue<int>(new { Id = evnt.RootId }, "tb_Thread", "ReplyCount", transaction);
                    connection.Insert(
                        new
                        {
                            Id = evnt.PostId,
                            ParentId = evnt.ParentId.Value,
                            RootId = evnt.RootId,
                            Subject = evnt.Subject,
                            Body = evnt.Body,
                            SectionId = evnt.SectionId,
                            AuthorId = evnt.AuthorId,
                            FloorIndex = replyCount + 1,
                            CreatedOn = evnt.CreatedOn
                        },
                        "tb_Post", transaction);
                    var authorName = connection.GetValue<string>(new { Id = evnt.AuthorId }, "tb_Account", "Name", transaction);
                    connection.Update(
                        new
                        {
                            ReplyCount = replyCount + 1,
                            MostRecentReplierId = evnt.AuthorId,
                            MostRecentReplierName = authorName,
                            MostRecentReplyCreatedOn = evnt.CreatedOn
                        },
                        new
                        {
                            Id = evnt.RootId
                        },
                        "tb_Thread", transaction);
                }
            });
        }
        public void Handle(PostBodyChanged evnt)
        {
            ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                connection.Update(new { Body = evnt.Body }, new { Id = evnt.PostId }, "tb_Post");
            });
        }
    }
}
