using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.CommandService.Providers
{
    public class EventTypeCodeProvider : AbstractTypeCodeProvider, IEventTypeCodeProvider
    {
        public EventTypeCodeProvider()
        {
            RegisterType<NewAccountRegisteredEvent>(100);
            RegisterType<AccountConfirmedEvent>(101);
            RegisterType<AccountRejectedEvent>(102);
        }
    }
}
