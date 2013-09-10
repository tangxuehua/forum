using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Post;
using Forum.Domain.Posts;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class PostCommandHandler :
        ICommandHandler<CreatePost>,
        ICommandHandler<ChangePostSubjectAndBody>
    {
        public void Handle(ICommandContext context, CreatePost command)
        {
            context.Add(new Post(command.Subject, command.Body, command.SectionId, command.AuthorId));
        }
        public void Handle(ICommandContext context, ChangePostSubjectAndBody command)
        {
            context.Get<Post>(command.PostId).ChangeSubjectAndBody(command.Subject, command.Body);
        }
    }
}
