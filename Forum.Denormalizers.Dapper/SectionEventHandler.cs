using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Events.Section;

namespace Forum.Denormalizers.Dapper
{
    [Component]
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
                    }, "tb_Section");
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
                    }, "tb_Section");
            }
        }
    }
}
