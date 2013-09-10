using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.Events.Post;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class PostEventHandler : BaseEventHandler,
        IEventHandler<PostCreated>,
        IEventHandler<PostSubjectAndBodyChanged>
    {
        public PostEventHandler(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void Handle(PostCreated evnt)
        {
            ConnectionFactory.CreateConnection().TryExecuteInTransaction((connection, transaction) =>
            {
                var authorName = connection.GetValue<string>(new { Id = evnt.AuthorId }, "tb_Account", "Name", transaction);
                connection.Insert(
                    new
                    {
                        Id = evnt.PostId,
                        Subject = evnt.Subject,
                        Body = evnt.Body,
                        SectionId = evnt.SectionId,
                        AuthorId = evnt.AuthorId,
                        AuthorName = authorName,
                        ReplyCount = 0,
                        CreatedOn = evnt.CreatedOn
                    },
                    "tb_Post", transaction);
            });
        }
        public void Handle(PostSubjectAndBodyChanged evnt)
        {
            ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                connection.Update(new { Subject = evnt.Subject, Body = evnt.Body }, new { Id = evnt.PostId }, "tb_Post");
            });
        }
    }
}
