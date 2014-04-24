using ECommon.IoC;
using ENode.Eventing;
using ENode.Infrastructure.Dapper;
using Forum.Events.Account;

namespace Forum.Denormalizers.Dapper
{
    [Component]
    public class AccountEventHandler : BaseEventHandler, IEventHandler<AccountCreated>
    {
        public void Handle(AccountCreated evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(new { Id = evnt.AggregateRootId, Name = evnt.Name, Password = evnt.Password }, "tb_Account");
            }
        }
    }
}
