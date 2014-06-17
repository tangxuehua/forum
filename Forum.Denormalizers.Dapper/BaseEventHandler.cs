using System;
using System.Data;
using System.Data.SqlClient;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public abstract class BaseEventHandler
    {
        protected void TryUpdateRecord(Func<IDbConnection, int> action)
        {
            using (var connection = GetConnection())
            {
                var result = action(connection);
                if (result != 1)
                {
                    throw new NoRowsUpdateException();
                }
            }
        }
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }

    public class NoRowsUpdateException : Exception
    {
    }
}
