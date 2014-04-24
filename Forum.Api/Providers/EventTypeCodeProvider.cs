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
            RegisterType<AccountCreatedEvent>(100);

            RegisterType<SectionCreatedEvent>(200);
            RegisterType<SectionUpdatedEvent>(201);

            RegisterType<PostCreatedEvent>(300);
            RegisterType<PostUpdatedEvent>(301);

            RegisterType<ReplyCreatedEvent>(400);
            RegisterType<ReplyBodyUpdatedEvent>(401);
        }
    }
}
