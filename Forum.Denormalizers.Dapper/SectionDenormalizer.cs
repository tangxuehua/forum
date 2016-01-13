using System.Threading.Tasks;
using ECommon.Components;
using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Forum.Domain.Sections;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public class SectionDenormalizer : AbstractDenormalizer,
        IMessageHandler<SectionCreatedEvent>,
        IMessageHandler<SectionNameChangedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(SectionCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name = evnt.Name,
                    CreatedOn = evnt.Timestamp,
                    UpdatedOn = evnt.Timestamp,
                    Version = evnt.Version
                }, Constants.SectionTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SectionNameChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Name,
                    UpdatedOn = evnt.Timestamp,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    Version = evnt.Version - 1
                }, Constants.SectionTable);
            });
        }
    }
}
