using ECommon.Components;
using ENode.Eventing;
using ENode.Infrastructure.Impl;
using Forum.Denormalizers.Dapper;

namespace Forum.Domain.Tests
{
    [Component]
    public class EventHandlerTypeCodeProvider : DefaultTypeCodeProvider<IEventHandler>
    {
        public EventHandlerTypeCodeProvider()
        {
            RegisterType<AccountEventHandler>(100);
            RegisterType<SectionEventHandler>(101);
            RegisterType<PostEventHandler>(102);
            RegisterType<ReplyEventHandler>(103);
            RegisterType<ReplyCreatedMessageSender>(104);
        }
    }
}
