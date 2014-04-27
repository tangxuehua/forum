using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using Forum.Domain.Posts;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component(LifeStyle.Singleton)]
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
                    }, Constants.PostTable);
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
                    }, Constants.PostTable);
            }
        }
    }
}
