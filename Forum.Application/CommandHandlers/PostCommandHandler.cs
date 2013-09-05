using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Application.Commands;
using Forum.Domain.Model;

namespace Forum.Application.CommandHandlers
{
    [Component]
    public class PostCommandHandler :
        ICommandHandler<CreateTopPost>,
        ICommandHandler<CreateReplyPost>,
        ICommandHandler<ChangePostBody>
    {
        public void Handle(ICommandContext context, CreateTopPost command)
        {
            context.Add(new Post(command.Subject, command.Body, null, null, command.SectionId, command.AuthorId));
        }
        public void Handle(ICommandContext context, CreateReplyPost command)
        {
            var parent = context.Get<Post>(command.ParentId);
            var root = context.Get<Post>(command.RootId);
            context.Add(new Post(null, command.Body, parent, root, command.SectionId, command.AuthorId));
        }
        public void Handle(ICommandContext context, ChangePostBody command)
        {
            context.Get<Post>(command.PostId).ChangeBody(command.Body);
        }
    }
}
