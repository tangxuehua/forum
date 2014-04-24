using System.Data;
using ECommon.IoC;
using ENode.Infrastructure.Sql;

namespace Forum.QueryServices.Dapper
{
    public abstract class BaseQueryService
    {
        private ISqlQueryDbConnectionFactory _connectionFactory;

        protected BaseQueryService()
        {
            _connectionFactory = ObjectContainer.Resolve<ISqlQueryDbConnectionFactory>();
        }

        protected IDbConnection GetConnection()
        {
            return _connectionFactory.CreateConnection();
        }
    }
}
