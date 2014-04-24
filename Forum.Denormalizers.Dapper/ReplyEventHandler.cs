using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Events.Reply;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class ReplyEventHandler : BaseEventHandler,
        IEventHandler<PostReplied>,
        IEventHandler<ReplyReplied>,
        IEventHandler<ReplyBodyChanged>
    {
        public void Handle(PostReplied evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.Info.ReplyId,
                        PostId = evnt.Info.PostId,
                        Body = evnt.Info.Body,
                        AuthorId = evnt.Info.AuthorId,
                        CreatedOn = evnt.Info.CreatedOn
                    },
                    "tb_Reply");
            }
        }
        public void Handle(ReplyReplied evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.Info.ReplyId,
                        ParentId = evnt.ParentReplyId,
                        PostId = evnt.Info.PostId,
                        Body = evnt.Info.Body,
                        AuthorId = evnt.Info.AuthorId,
                        CreatedOn = evnt.Info.CreatedOn
                    },
                    "tb_Reply");
            }
        }
        public void Handle(ReplyBodyChanged evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(
                    new { Body = evnt.Body },
                    new { Id = evnt.AggregateRootId },
                    "tb_Reply");
            }
        }
    }
}
