using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Domain;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class AccountCommandHandler :
        ICommandHandler<RegisterNewAccountCommand>,
        ICommandHandler<ConfirmAccountCommand>,
        ICommandHandler<RejectAccountCommand>
    {
        private readonly AggregateRootFactory _factory;

        public AccountCommandHandler(AggregateRootFactory factory)
        {
            _factory = factory;
        }

        public void Handle(ICommandContext context, RegisterNewAccountCommand command)
        {
            context.Add(_factory.CreateAccount(command.Name, command.Password));
        }
        public void Handle(ICommandContext context, ConfirmAccountCommand command)
        {
            context.Get<Account>(command.AggregateRootId).Confirm();
        }
        public void Handle(ICommandContext context, RejectAccountCommand command)
        {
            context.Get<Account>(command.AggregateRootId).Reject(command.ReasonCode);
        }
    }
}
