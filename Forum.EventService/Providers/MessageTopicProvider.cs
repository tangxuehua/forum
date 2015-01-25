using ECommon.Components;
using ENode.EQueue;
using ENode.Messaging;
using Forum.Denormalizers.Dapper;

namespace Forum.EventService.Providers
{
    [Component]
    public class MessageTopicProvider : AbstractTopicProvider<IMessage>
    {
        public MessageTopicProvider()
        {
            RegisterTopic("ReplyMessageTopic", typeof(ReplyCreatedMessage));
        }
    }
}
