using ECommon.Components;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Messaging;
using Forum.Domain.Replies;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class ReplyCreatedMessageSender : IEventHandler<ReplyCreatedEvent>
    {
        private IPublisher<IMessage> _messagePublisher;

        public ReplyCreatedMessageSender(IPublisher<IMessage> messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Handle(IHandlingContext context, ReplyCreatedEvent evnt)
        {
            _messagePublisher.Publish(new ReplyCreatedMessage
            {
                SourceId = evnt.AggregateRootId,
                Timestamp = evnt.Timestamp,
                PostId = evnt.PostId,
                AuthorId = evnt.AuthorId,
                ParentId = evnt.ParentId
            });
        }
    }
}
