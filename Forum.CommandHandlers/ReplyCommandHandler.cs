using ENode.Commanding;
using Forum.Commands.Replies;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    public class ReplyCommandHandler :
        ICommandHandler<CreateReplyCommand>,
        ICommandHandler<ChangeReplyBodyCommand>
    {
        public void Handle(ICommandContext context, CreateReplyCommand command)
        {
            Reply parent = null;
            if (!string.IsNullOrEmpty(command.ParentId))
            {
                parent = context.Get<Reply>(command.ParentId);
            }
            context.Add(new Reply(command.AggregateRootId, command.PostId, parent, command.AuthorId, command.Body));
        }
        public void Handle(ICommandContext context, ChangeReplyBodyCommand command)
        {
            context.Get<Reply>(command.AggregateRootId).ChangeBody(command.Body);
        }
    }
}
