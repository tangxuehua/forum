using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Domain.Accounts;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class AccountEventHandler : BaseEventHandler, IEventHandler<AccountCreatedEvent>
    {
        public void Handle(AccountCreatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        Name = evnt.Name,
                        Password = evnt.Password,
                        CreatedOn = evnt.Timestamp,
                        UpdatedOn = evnt.Timestamp
                    }, "tb_Account");
            }
        }
    }
}
