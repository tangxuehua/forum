using ENode.Eventing;
using ENode.Infrastructure;
using Forum.ProcessManagers;

namespace Forum.CommandService.Providers
{
    public class EventHandlerTypeCodeProvider : AbstractTypeCodeProvider, IEventHandlerTypeCodeProvider
    {
        public EventHandlerTypeCodeProvider()
        {
            RegisterType<RegistrationProcessManager>(100);
        }
    }
}
