using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using Forum.Domain.Sections;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class SectionEventHandler : BaseEventHandler,
        IEventHandler<SectionCreatedEvent>,
        IEventHandler<SectionUpdatedEvent>
    {
        public void Handle(SectionCreatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Name = evnt.Name,
                        CreatedOn = evnt.Timestamp,
                        UpdatedOn = evnt.Timestamp
                    }, Constants.SectionTable);
            }
        }
        public void Handle(SectionUpdatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(
                    new
                    {
                        Name = evnt.Name,
                        UpdatedOn = evnt.Timestamp
                    },
                    new
                    {
                        Id = evnt.AggregateRootId
                    }, Constants.SectionTable);
            }
        }
    }
}
