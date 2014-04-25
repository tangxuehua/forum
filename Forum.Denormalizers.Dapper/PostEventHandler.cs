using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Domain.Posts;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class PostEventHandler : BaseEventHandler,
        IEventHandler<PostCreatedEvent>,
        IEventHandler<PostUpdatedEvent>
    {
        public void Handle(PostCreatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Subject = evnt.Subject,
                        Body = evnt.Body,
                        SectionId = evnt.SectionId,
                        AuthorId = evnt.AuthorId,
                        CreatedOn = evnt.Timestamp,
                        UpdatedOn = evnt.Timestamp
                    }, "tb_Post");
            }
        }
        public void Handle(PostUpdatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(
                    new
                    {
                        Subject = evnt.Subject,
                        Body = evnt.Body,
                        UpdatedOn = evnt.Timestamp
                    },
                    new
                    {
                        Id = evnt.AggregateRootId
                    }, "tb_Post");
            }
        }
    }
}
