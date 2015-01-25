using ECommon.Components;
using ENode.Infrastructure.Impl;
using ENode.Messaging;
using Forum.Denormalizers.Dapper;

namespace Forum.Domain.Tests
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
