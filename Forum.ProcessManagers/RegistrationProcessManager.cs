using ECommon.Components;
using ENode.Eventing;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.ProcessManagers
{
    /// <summary>注册流程管理器，负责实现注册的流程
    /// </summary>
    [Component(LifeStyle.Singleton)]
    public class RegistrationProcessManager : IEventHandler<NewAccountRegisteredEvent>
    {
        private ValidateAccountService _validateAccountService;

        public RegistrationProcessManager(ValidateAccountService validateAccountService)
        {
            _validateAccountService = validateAccountService;
        }

        public void Handle(IEventContext context, NewAccountRegisteredEvent evnt)
        {
            var result = _validateAccountService.ValidateAccountNameUniqueness(evnt.AccountInfo.Name, evnt.AggregateRootId);
            if (result == AccountNameUniquenessValidateResult.Success)
            {
                context.AddCommand(new ConfirmAccountCommand(evnt.AggregateRootId));
            }
            else if (result == AccountNameUniquenessValidateResult.DuplicateAccountName)
            {
                context.AddCommand(new RejectAccountCommand(evnt.AggregateRootId, ErrorCodes.DuplicateAccount));
            }
        }
    }
}
