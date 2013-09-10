using ENode.Infrastructure.Sql;

namespace Forum.QueryServices.Dapper
{
    public abstract class BaseQueryService
    {
        protected ISqlQueryDbConnectionFactory ConnectionFactory { get; private set; }

        protected BaseQueryService(ISqlQueryDbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }
    }
}
