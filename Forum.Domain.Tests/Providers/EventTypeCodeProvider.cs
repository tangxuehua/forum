using ECommon.Components;
using ENode.Eventing;
using ENode.Infrastructure.Impl;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.Domain.Tests
{
    [Component]
    public class EventTypeCodeProvider : DefaultTypeCodeProvider<IEvent>
    {
        public EventTypeCodeProvider()
        {
            RegisterType<NewAccountRegisteredEvent>(100);

            RegisterType<SectionCreatedEvent>(200);
            RegisterType<SectionNameChangedEvent>(201);

            RegisterType<PostCreatedEvent>(300);
            RegisterType<PostUpdatedEvent>(301);

            RegisterType<ReplyCreatedEvent>(400);
            RegisterType<ReplyBodyChangedEvent>(401);
        }
    }
}
