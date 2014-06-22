using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class RegistrationCommandHandler :
        ICommandHandler<StartRegistrationCommand>,
        ICommandHandler<ConfirmRegistrationCommand>,
        ICommandHandler<CompleteRegistrationCommand>,
        ICommandHandler<CancelRegistrationCommand>
    {
        private readonly AggregateRootFactory _factory;

        public RegistrationCommandHandler(AggregateRootFactory factory)
        {
            _factory = factory;
        }

        public void Handle(ICommandContext context, StartRegistrationCommand command)
        {
            context.Add(_factory.CreateRegistration(command.AccountName, command.AccountPassword));
        }
        public void Handle(ICommandContext context, ConfirmRegistrationCommand command)
        {
            context.Get<Registration>(command.AggregateRootId).Confirm();
        }
        public void Handle(ICommandContext context, CompleteRegistrationCommand command)
        {
            context.Get<Registration>(command.AggregateRootId).Complete();
        }
        public void Handle(ICommandContext context, CancelRegistrationCommand command)
        {
            context.Get<Registration>(command.AggregateRootId).Cancel(command.ErrorCode);
        }
    }
}
