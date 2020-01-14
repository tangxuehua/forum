using System.Threading.Tasks;
using ENode.Commanding;
using ENode.Messaging;
using Forum.Commands.Posts;
using Forum.Domain.Replies;

namespace Forum.ProcessManagers
{
    public class CreateReplyProcessManager : IMessageHandler<ReplyCreatedEvent>
    {
        private ICommandService _commandService;

        public CreateReplyProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public Task HandleAsync(ReplyCreatedEvent evnt)
        {
            return _commandService.SendAsync(new AcceptNewReplyCommand(evnt.PostId, evnt.AggregateRootId));
        }
    }
}
