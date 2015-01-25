using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Sections;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class SectionEventHandler : BaseHandler,
        IEventHandler<SectionCreatedEvent>,
        IEventHandler<SectionNameChangedEvent>
    {
        public void Handle(IHandlingContext context, SectionCreatedEvent evnt)
        {
            TryDBAction(connection =>
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Name = evnt.Name,
                        CreatedOn = evnt.Timestamp,
                        UpdatedOn = evnt.Timestamp,
                        Version = evnt.Version
                    }, Constants.SectionTable);
            }, "InsertSection");
        }
        public void Handle(IHandlingContext context, SectionNameChangedEvent evnt)
        {
            TryDBAction(connection =>
            {
                connection.Update(
                    new
                    {
                        Name = evnt.Name,
                        UpdatedOn = evnt.Timestamp,
                        Version = evnt.Version
                    },
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Version = evnt.Version - 1
                    }, Constants.SectionTable);
            }, "UpdateSection");
        }
    }
}
