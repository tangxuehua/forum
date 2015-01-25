using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Posts;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class PostEventHandler : BaseHandler,
        IEventHandler<PostCreatedEvent>,
        IEventHandler<PostUpdatedEvent>
    {
        public void Handle(IHandlingContext context, PostCreatedEvent evnt)
        {
            TryInsertRecord(connection =>
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
                        UpdatedOn = evnt.Timestamp,
                        LastUpdateTime = evnt.Timestamp,
                        ReplyCount = 0,
                        Version = evnt.Version
                    }, Constants.PostTable);
            }, "InsertPost");
        }
        public void Handle(IHandlingContext context, PostUpdatedEvent evnt)
        {
            TryDBAction(connection =>
            {
                connection.Update(
                    new
                    {
                        Subject = evnt.Subject,
                        Body = evnt.Body,
                        UpdatedOn = evnt.Timestamp,
                        Version = evnt.Version
                    },
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Version = evnt.Version - 1
                    }, Constants.PostTable);
            }, "UpdatePost");
        }
    }
}
