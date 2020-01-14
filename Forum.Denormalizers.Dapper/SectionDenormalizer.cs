using System.Threading.Tasks;
using ECommon.Dapper;
using ENode.Messaging;
using Forum.Domain.Sections;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public class SectionDenormalizer : AbstractDenormalizer,
        IMessageHandler<SectionCreatedEvent>,
        IMessageHandler<SectionChangedEvent>
    {
        public Task HandleAsync(SectionCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name = evnt.Name,
                    CreatedOn = evnt.Timestamp,
                    UpdatedOn = evnt.Timestamp,
                    Description = evnt.Description,
                    Version = evnt.Version
                }, Constants.SectionTable);
            });
        }
        public Task HandleAsync(SectionChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Name,
                    UpdatedOn = evnt.Timestamp,
                    Description = evnt.Description,
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
