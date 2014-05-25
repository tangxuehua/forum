using ECommon.Components;
using ENode.Eventing;

namespace Forum.Domain.Accounts
{
    [Component(LifeStyle.Singleton)]
    public class CheckAccountNameUniqueService : IEventSynchronizer<AccountCreatedEvent>
    {
        private readonly IRegistrationRepository _registrationRepository;

        public CheckAccountNameUniqueService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public void OnBeforePersisting(AccountCreatedEvent evnt)
        {
            _registrationRepository.Add(new Registration(evnt.AggregateRootId, evnt.Name));
        }
        public void OnAfterPersisted(AccountCreatedEvent evnt)
        {
            _registrationRepository.Update(_registrationRepository.GetByAccountName(evnt.Name).ConfirmStatus());
        }
    }
}
