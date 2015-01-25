using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Replies;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class ReplyEventHandler : BaseHandler,
        IEventHandler<ReplyCreatedEvent>,
        IEventHandler<ReplyBodyChangedEvent>
    {
        public void Handle(IHandlingContext context, ReplyCreatedEvent evnt)
        {
            TryInsertRecord(connection =>
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        PostId = evnt.PostId,
                        ParentId = evnt.ParentId,
                        AuthorId = evnt.AuthorId,
                        Body = evnt.Body,
                        CreatedOn = evnt.Timestamp,
                        UpdatedOn = evnt.Timestamp,
                        Version = evnt.Version
                    },
                    Constants.ReplyTable);
            }, "InsertReply");
        }
        public void Handle(IHandlingContext context, ReplyBodyChangedEvent evnt)
        {
            TryDBAction(connection =>
            {
                connection.Update(
                    new
                    {
                        Body = evnt.Body,
                        UpdatedOn = evnt.Timestamp,
                        Version = evnt.Version
                    },
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Version = evnt.Version - 1
                    }, Constants.ReplyTable);
            }, "UpdateReplyBody");
        }
    }
}
