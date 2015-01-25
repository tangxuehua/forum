using ECommon.Components;
using ENode.EQueue;
using ENode.Messaging;
using Forum.Denormalizers.Dapper;

namespace Forum.Domain.Tests
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
