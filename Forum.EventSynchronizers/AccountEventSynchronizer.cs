using ECommon.IoC;
using ENode.Eventing;
using Forum.Domain.Accounts;

namespace Forum.EventSynchronizers
{
    [Component]
    public class AccountEventSynchronizer : IEventSynchronizer<AccountCreatedEvent>
    {
        private readonly IRegistrationRepository _registrationRepository;

        public AccountEventSynchronizer(IRegistrationRepository registrationRepository)
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
