using ECommon.IoC;
using ENode.Commanding;
using Forum.Commands.Posts;
using Forum.Domain.Posts;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class PostCommandHandler :
        ICommandHandler<CreatePostCommand>,
        ICommandHandler<UpdatePostCommand>
    {
        public void Handle(ICommandContext context, CreatePostCommand command)
        {
            context.Add(new Post(command.AggregateRootId, command.Subject, command.Body, command.SectionId, command.AuthorId));
        }
        public void Handle(ICommandContext context, UpdatePostCommand command)
        {
            context.Get<Post>(command.AggregateRootId).Update(command.Subject, command.Body);
        }
    }
}
