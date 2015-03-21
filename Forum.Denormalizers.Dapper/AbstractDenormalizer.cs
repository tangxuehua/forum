using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ECommon.Components;
using ECommon.Logging;
using ENode.Infrastructure;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public abstract class AbstractDenormalizer
    {
        private static readonly IOHelper _ioHelper = ObjectContainer.Resolve<IOHelper>();
        protected readonly ILogger _logger;

        public AbstractDenormalizer()
        {
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName);
        }

        protected async Task<AsyncTaskResult> TryInsertRecordAsync(Func<IDbConnection, Task<long>> action)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    await action(connection);
                    return AsyncTaskResult.Success;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)  //主键冲突，忽略即可；出现这种情况，是因为同一个消息的重复处理
                {
                    return AsyncTaskResult.Success;
                }
                throw;
            }
        }
        protected async Task<AsyncTaskResult> TryUpdateRecordAsync(Func<IDbConnection, Task<int>> action)
        {
            using (var connection = GetConnection())
            {
                await action(connection);
                return AsyncTaskResult.Success;
            }
        }
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
