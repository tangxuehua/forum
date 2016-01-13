using System.Threading.Tasks;
using ECommon.Components;
using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Forum.Domain.Posts;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public class PostDenormalizer : AbstractDenormalizer,
        IMessageHandler<PostCreatedEvent>,
        IMessageHandler<PostUpdatedEvent>,
        IMessageHandler<PostReplyStatisticInfoChangedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(PostCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
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
            });
        }
        public Task<AsyncTaskResult> HandleAsync(PostUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Subject = evnt.Subject,
                    Body = evnt.Body,
                    UpdatedOn = evnt.Timestamp,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    Version = evnt.Version - 1
                }, Constants.PostTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(PostReplyStatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    MostRecentReplyId = evnt.StatisticInfo.LastReplyId,
                    MostRecentReplierId = evnt.StatisticInfo.LastReplyAuthorId,
                    LastUpdateTime = evnt.StatisticInfo.LastReplyTime,
                    ReplyCount = evnt.StatisticInfo.ReplyCount,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    Version = evnt.Version - 1
                }, Constants.PostTable);
            });
        }
    }
}
