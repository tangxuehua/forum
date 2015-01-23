using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class AccountEventHandler : BaseEventHandler, IEventHandler<NewAccountRegisteredEvent>
    {
        public void Handle(IHandlingContext context, NewAccountRegisteredEvent evnt)
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
