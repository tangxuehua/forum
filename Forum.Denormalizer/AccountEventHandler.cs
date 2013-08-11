using Dapper;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Events;

namespace Forum.Denormalizer
{
    [Component]
    public class AccountEventHandler : BaseEventHandler,
        IEventHandler<AccountCreated> {
        public AccountEventHandler(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void Handle(AccountCreated evnt) {
            ConnectionFactory.CreateConnection().TryExecute((connection) => {
                connection.Insert(new { Id = evnt.AccountId, Name = evnt.Name, Password = evnt.Password }, "tb_Account");
            });
        }
    }
}
