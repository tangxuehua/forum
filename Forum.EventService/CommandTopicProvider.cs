using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Forum.Commands.Posts;

namespace Forum.EventService.Providers
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public CommandTopicProvider()
        {
            RegisterTopic("PostCommandTopic", typeof(AcceptNewReplyCommand));
        }
    }
}
