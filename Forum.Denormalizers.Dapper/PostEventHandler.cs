using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Events.Post;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class PostEventHandler : BaseEventHandler,
        IEventHandler<PostCreated>,
        IEventHandler<PostSubjectAndBodyChanged>
    {
        public void Handle(PostCreated evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Subject = evnt.Subject,
                        Body = evnt.Body,
                        SectionId = evnt.SectionId,
                        AuthorId = evnt.AuthorId,
                        CreatedOn = evnt.CreatedOn
                    },
                    "tb_Post");
            }
        }
        public void Handle(PostSubjectAndBodyChanged evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(
                    new { Subject = evnt.Subject, Body = evnt.Body },
                    new { Id = evnt.AggregateRootId },
                    "tb_Post");
            }
        }
    }
}
