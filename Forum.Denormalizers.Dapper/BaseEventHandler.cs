using System.Data;
using System.Data.SqlClient;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public abstract class BaseEventHandler
    {
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
