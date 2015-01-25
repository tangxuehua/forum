using ECommon.Components;
using ENode.Infrastructure.Impl;
using ENode.Messaging;
using Forum.Denormalizers.Dapper;

namespace Forum.EventService.Providers
{
    [Component]
    public class MessageTypeCodeProvider : DefaultTypeCodeProvider<IMessage>
    {
        public MessageTypeCodeProvider()
        {
            RegisterType<ReplyCreatedMessage>(100);
        }
    }
}
