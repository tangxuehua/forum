using System.Threading.Tasks;
using ENode.Commanding;
using Forum.Commands.Posts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;

namespace Forum.CommandHandlers
{
    public class PostCommandHandler :
        ICommandHandler<CreatePostCommand>,
        ICommandHandler<UpdatePostCommand>,
        ICommandHandler<AcceptNewReplyCommand>
    {
        public Task HandleAsync(ICommandContext context, CreatePostCommand command)
        {
            return context.AddAsync(new Post(command.AggregateRootId, command.Subject, command.Body, command.SectionId, command.AuthorId));
        }
        public async Task HandleAsync(ICommandContext context, UpdatePostCommand command)
        {
            var post = await context.GetAsync<Post>(command.AggregateRootId);
            post.Update(command.Subject, command.Body);
        }
        public async Task HandleAsync(ICommandContext context, AcceptNewReplyCommand command)
        {
            var post = await context.GetAsync<Post>(command.AggregateRootId);
            var reply = await context.GetAsync<Reply>(command.ReplyId);
            post.AcceptNewReply(reply);
        }
    }
}
