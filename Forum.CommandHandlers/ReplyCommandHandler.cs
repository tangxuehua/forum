using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Reply;
using Forum.Domain.Posts;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class ReplyCommandHandler :
        ICommandHandler<CreateReply>,
        ICommandHandler<ChangeReplyBody>
    {
        public void Handle(ICommandContext context, CreateReply command)
        {
            if (command.ParentId != null)
            {
                var parent = context.Get<Reply>(command.ParentId.Value);
                context.Add(new Reply(command.Body, parent, command.AuthorId));
            }
            else
            {
                var post = context.Get<Post>(command.PostId);
                context.Add(new Reply(command.Body, post, command.AuthorId));
            }
        }
        public void Handle(ICommandContext context, ChangeReplyBody command)
        {
            context.Get<Reply>(command.ReplyId).ChangeBody(command.Body);
        }
    }
}
