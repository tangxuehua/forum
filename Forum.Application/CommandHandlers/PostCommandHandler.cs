using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Application.Commands;
using Forum.Domain.Model;

namespace Forum.Application.CommandHandlers
{
    [Component]
    public class PostCommandHandler :
        ICommandHandler<CreatePost>,
        ICommandHandler<ChangePostBody>
    {
        public void Handle(ICommandContext context, CreatePost command)
        {
            context.Add(new Post(command.PostInfo));
        }
        public void Handle(ICommandContext context, ChangePostBody command)
        {
            context.Get<Post>(command.PostId).ChangeBody(command.Body);
        }
    }
}
