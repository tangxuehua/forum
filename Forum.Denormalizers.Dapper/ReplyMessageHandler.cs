using Dapper;
using ECommon.Components;
using ENode.Infrastructure;
using ENode.Messaging;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class ReplyMessageHandler : BaseHandler, IMessageHandler<ReplyCreatedMessage>
    {
        public void Handle(IHandlingContext context, ReplyCreatedMessage message)
        {
            TryDBAction(connection =>
            {
                connection.Execute(@"Update Post Set
                    MostRecentReplyId = @MostRecentReplyId,
                    MostRecentReplierId = @MostRecentReplierId,
                    LastUpdateTime = @LastUpdateTime,
                    ReplyCount = ReplyCount + 1
                    Where Id = @Id",
                new
                {
                    MostRecentReplyId = message.SourceId,
                    MostRecentReplierId = message.AuthorId,
                    LastUpdateTime = message.Timestamp,
                    Id = message.PostId
                });
            }, "UpdatePostStatisticInfo");
        }
    }
}
