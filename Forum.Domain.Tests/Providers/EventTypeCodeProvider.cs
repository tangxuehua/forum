using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.Domain.Tests
{
    public class EventTypeCodeProvider : AbstractTypeCodeProvider, IEventTypeCodeProvider
    {
        public EventTypeCodeProvider()
        {
            RegisterType<RegistrationStartedEvent>(100);
            RegisterType<RegistrationConfirmedEvent>(101);
            RegisterType<RegistrationCompletedEvent>(102);
            RegisterType<RegistrationCanceledEvent>(103);

            RegisterType<AccountCreatedEvent>(200);

            RegisterType<SectionCreatedEvent>(300);
            RegisterType<SectionUpdatedEvent>(301);

            RegisterType<PostCreatedEvent>(400);
            RegisterType<PostUpdatedEvent>(401);

            RegisterType<ReplyCreatedEvent>(500);
            RegisterType<ReplyBodyUpdatedEvent>(501);
        }
    }
}
