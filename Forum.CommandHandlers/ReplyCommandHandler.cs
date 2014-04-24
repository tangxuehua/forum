using ECommon.IoC;
using ENode.Commanding;
using Forum.Commands.Reply;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class ReplyCommandHandler :
        ICommandHandler<CreateReplyCommand>,
        ICommandHandler<UpdateReplyBodyCommand>
    {
        public void Handle(ICommandContext context, CreateReplyCommand command)
        {
            context.Add(new Reply(command.AggregateRootId, command.PostId, command.ParentId, command.AuthorId, command.Body));
        }
        public void Handle(ICommandContext context, UpdateReplyBodyCommand command)
        {
            context.Get<Reply>(command.AggregateRootId).UpdateBody(command.Body);
        }
    }
}
