using System.Data;
using ECommon.IoC;
using ENode.Infrastructure.Sql;

namespace Forum.Denormalizers.Dapper
{
    public abstract class BaseEventHandler
    {
        private ISqlQueryDbConnectionFactory _connectionFactory;

        protected BaseEventHandler()
        {
            _connectionFactory = ObjectContainer.Resolve<ISqlQueryDbConnectionFactory>();
        }

        protected IDbConnection GetConnection()
        {
            return _connectionFactory.CreateConnection();
        }
    }
}
