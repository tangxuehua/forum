using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Denormalizers.Dapper;

namespace Forum.EventService.Providers
{
    public class EventHandlerTypeCodeProvider : AbstractTypeCodeProvider, IEventHandlerTypeCodeProvider
    {
        public EventHandlerTypeCodeProvider()
        {
            RegisterType<AccountEventHandler>(101);
            RegisterType<SectionEventHandler>(102);
            RegisterType<PostEventHandler>(103);
            RegisterType<ReplyEventHandler>(104);
        }
    }
}
