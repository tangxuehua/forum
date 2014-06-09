using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class AccountEventHandler : BaseEventHandler, IEventHandler<AccountCreatedEvent>
    {
        public void Handle(IEventContext context, AccountCreatedEvent evnt)
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
                        UpdatedOn = evnt.Timestamp,
                        Version = evnt.Version
                    }, Constants.AccountTable);
            }
        }
    }
}
