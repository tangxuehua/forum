using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.Domain.Events;

namespace Forum.Denormalizer
{
    [Component]
    public class SectionEventHandler : BaseEventHandler,
        IEventHandler<SectionCreated>,
        IEventHandler<SectionNameChanged>
    {
        public SectionEventHandler(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void Handle(SectionCreated evnt)
        {
            ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                connection.Insert(new { Id = evnt.SectionId, Name = evnt.Name }, "tb_Section");
            });
        }
        public void Handle(SectionNameChanged evnt)
        {
            ConnectionFactory.CreateConnection().TryExecute(connection =>
            {
                connection.Update(new { Name = evnt.Name }, new { Id = evnt.SectionId }, "tb_Section");
            });
        }
    }
}
