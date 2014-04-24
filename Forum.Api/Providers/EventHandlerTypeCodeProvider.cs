using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Denormalizers.Dapper;

namespace Forum.Api.Providers
{
    public class EventHandlerTypeCodeProvider : AbstractTypeCodeProvider, IEventHandlerTypeCodeProvider
    {
        public EventHandlerTypeCodeProvider()
        {
            RegisterType<AccountEventHandler>(100);
            RegisterType<SectionEventHandler>(101);
            RegisterType<PostEventHandler>(102);
            RegisterType<ReplyEventHandler>(103);
        }
    }
}
