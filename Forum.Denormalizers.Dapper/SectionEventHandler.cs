using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Events.Section;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class SectionEventHandler : BaseEventHandler,
        IEventHandler<SectionCreated>,
        IEventHandler<SectionNameChanged>
    {
        public void Handle(SectionCreated evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(new { Id = evnt.AggregateRootId, Name = evnt.Name }, "tb_Section");
            }
        }
        public void Handle(SectionNameChanged evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(new { Name = evnt.Name }, new { Id = evnt.AggregateRootId }, "tb_Section");
            }
        }
    }
}
