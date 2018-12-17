using System.Threading.Tasks;
using ENode.Commanding;
using Forum.Commands.Replies;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    public class ReplyCommandHandler :
        ICommandHandler<CreateReplyCommand>,
        ICommandHandler<ChangeReplyBodyCommand>
    {
        public async Task HandleAsync(ICommandContext context, CreateReplyCommand command)
        {
            Reply parent = null;
            if (!string.IsNullOrEmpty(command.ParentId))
            {
                parent = await context.GetAsync<Reply>(command.ParentId);
            }
            context.Add(new Reply(command.AggregateRootId, command.PostId, parent, command.AuthorId, command.Body));
        }
        public async Task HandleAsync(ICommandContext context, ChangeReplyBodyCommand command)
        {
            var reply = await context.GetAsync<Reply>(command.AggregateRootId);
            reply.ChangeBody(command.Body);
        }
    }
}
