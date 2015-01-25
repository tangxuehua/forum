using System;
using System.Data;
using System.Data.SqlClient;
using ECommon.Components;
using ECommon.Logging;
using ENode.Infrastructure;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public abstract class BaseHandler
    {
        private static readonly IOHelper _ioHelper = ObjectContainer.Resolve<IOHelper>();
        protected readonly ILogger _logger;

        public BaseHandler()
        {
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName);
        }

        protected void TryDBAction(Action<IDbConnection> action, string actionName)
        {
            using (var connection = GetConnection())
            {
                _ioHelper.TryIOAction(() => action(connection), actionName);
            }
        }
        protected void TryInsertRecord(Action<IDbConnection> action, string actionName)
        {
            TryDBAction(connection =>
            {
                try
                {
                    action(connection);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        _logger.ErrorFormat("Try insert record failed, duplicated primary key. current actionName:{0}", actionName);
                        return;
                    }
                    throw;
                }
            }, actionName);
        }
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
