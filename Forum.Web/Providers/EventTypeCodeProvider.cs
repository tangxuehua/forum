using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.Web.Providers
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
