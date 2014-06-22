using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Posts;
using Forum.Domain;
using Forum.Domain.Posts;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class PostCommandHandler :
        ICommandHandler<CreatePostCommand>,
        ICommandHandler<UpdatePostCommand>
    {
        private readonly AggregateRootFactory _factory;

        public PostCommandHandler(AggregateRootFactory factory)
        {
            _factory = factory;
        }

        public void Handle(ICommandContext context, CreatePostCommand command)
        {
            context.Add(_factory.CreatePost(command.Subject, command.Body, command.SectionId, command.AuthorId));
        }
        public void Handle(ICommandContext context, UpdatePostCommand command)
        {
            context.Get<Post>(command.AggregateRootId).Update(command.Subject, command.Body);
        }
    }
}
