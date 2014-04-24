using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Events.Reply;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class ReplyEventHandler : BaseEventHandler,
        IEventHandler<ReplyCreatedEvent>,
        IEventHandler<ReplyBodyUpdatedEvent>
    {
        public void Handle(ReplyCreatedEvent evnt)
        {
            using (var connection = GetConnection())
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
                        UpdatedOn = evnt.Timestamp
                    },
                    "tb_Reply");
            }
        }
        public void Handle(ReplyBodyUpdatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(
                    new
                    {
                        Body = evnt.Body,
                        UpdatedOn = evnt.Timestamp
                    },
                    new
                    {
                        Id = evnt.AggregateRootId
                    }, "tb_Reply");
            }
        }
    }
}
