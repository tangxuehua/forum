using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Events.Account;
using Forum.Events.Post;
using Forum.Events.Reply;
using Forum.Events.Section;

namespace Forum.Api.Providers
{
    public class EventTypeCodeProvider : AbstractTypeCodeProvider, IEventTypeCodeProvider
    {
        public EventTypeCodeProvider()
        {
            RegisterType<AccountCreated>(100);

            RegisterType<SectionCreated>(200);
            RegisterType<SectionNameChanged>(201);

            RegisterType<PostCreated>(300);
            RegisterType<PostSubjectAndBodyChanged>(301);

            RegisterType<PostReplied>(400);
            RegisterType<ReplyReplied>(401);
            RegisterType<ReplyBodyChanged>(402);
        }
    }
}
