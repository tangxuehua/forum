using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Denormalizers.Dapper;
using Forum.ProcessManagers;

namespace Forum.Domain.Tests
{
    public class EventHandlerTypeCodeProvider : AbstractTypeCodeProvider, IEventHandlerTypeCodeProvider
    {
        public EventHandlerTypeCodeProvider()
        {
            RegisterType<RegistrationProcessManager>(100);
            RegisterType<AccountEventHandler>(101);
            RegisterType<SectionEventHandler>(102);
            RegisterType<PostEventHandler>(103);
            RegisterType<ReplyEventHandler>(104);
        }
    }
}
