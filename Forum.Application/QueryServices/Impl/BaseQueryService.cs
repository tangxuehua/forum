using ENode.Infrastructure.Sql;

namespace Forum.Application.QueryServices.Impl
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
