using ECommon.IoC;
using ENode.Commanding;
using Forum.Commands.Reply;
using Forum.Domain.Posts;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class ReplyCommandHandler :
        ICommandHandler<CreateReplyCommand>,
        ICommandHandler<ChangeReplyCommand>
    {
        public void Handle(ICommandContext context, CreateReplyCommand command)
        {
            if (command.ParentId != null)
            {
                var parent = context.Get<Reply>(command.ParentId);
                context.Add(new Reply(command.AggregateRootId, command.Body, parent, command.AuthorId));
            }
            else
            {
                var post = context.Get<Post>(command.PostId);
                context.Add(new Reply(command.AggregateRootId, command.Body, post, command.AuthorId));
            }
        }
        public void Handle(ICommandContext context, ChangeReplyCommand command)
        {
            context.Get<Reply>(command.AggregateRootId).ChangeBody(command.Body);
        }
    }
}
