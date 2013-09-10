using ENode.Eventing;
using Forum.Events.Reply;

namespace Forum.EventHandlers
{
    public class SendEmailHandler : IEventHandler<PostReplied>, IEventHandler<ReplyReplied>
    {
        public void Handle(PostReplied evnt)
        {
            //Here, we can send an email to the post author when a post has a new reply.
        }

        public void Handle(ReplyReplied evnt)
        {
            //Here, we can send an email to the parent reply author when a parent reply has a new reply.
        }
    }
}
