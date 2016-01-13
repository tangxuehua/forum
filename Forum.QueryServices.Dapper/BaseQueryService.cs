using System.Data;
using System.Data.SqlClient;
using Forum.Infrastructure;

namespace Forum.QueryServices.Dapper
{
    public abstract class BaseQueryService
    {
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ForumConnectionString);
        }
    }
}
