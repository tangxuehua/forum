using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Posts;
using Forum.Domain;
using Forum.Domain.Posts;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    [Component]
    public class PostCommandHandler :
        ICommandHandler<CreatePostCommand>,
        ICommandHandler<UpdatePostCommand>,
        ICommandHandler<AcceptNewReplyCommand>
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
        public void Handle(ICommandContext context, AcceptNewReplyCommand command)
        {
            context.Get<Post>(command.AggregateRootId).AcceptNewReply(context.Get<Reply>(command.ReplyId));
        }
    }
}
