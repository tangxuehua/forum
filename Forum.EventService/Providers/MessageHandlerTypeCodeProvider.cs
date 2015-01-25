using ECommon.Components;
using ENode.Infrastructure.Impl;
using ENode.Messaging;
using Forum.Denormalizers.Dapper;

namespace Forum.EventService.Providers
{
    [Component]
    public class MessageHandlerTypeCodeProvider : DefaultTypeCodeProvider<IMessageHandler>
    {
        public MessageHandlerTypeCodeProvider()
        {
            RegisterType<ReplyMessageHandler>(100);
        }
    }
}
