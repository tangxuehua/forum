using ENode.Infrastructure.Sql;

namespace Forum.Denormalizers.Dapper
{
    public abstract class BaseEventHandler
    {
        protected ISqlQueryDbConnectionFactory ConnectionFactory { get; private set; }

        protected BaseEventHandler(ISqlQueryDbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }
    }
}
