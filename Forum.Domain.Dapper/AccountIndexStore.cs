using System.Data.SqlClient;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Domain.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class AccountIndexStore : IAccountIndexStore
    {
        public AccountIndex FindByAccountId(string accountId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var data = connection.QueryList(new { AccountId = accountId }, Constants.AccountIndexTable).SingleOrDefault();
                if (data != null)
                {
                    var indexId = data.IndexId as string;
                    var accountName = data.AccountName as string;
                    return new AccountIndex(indexId, accountId, accountName);
                }
                return null;
            }
        }
        public AccountIndex FindByAccountName(string accountName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var data = connection.QueryList(new { AccountName = accountName }, Constants.AccountIndexTable).SingleOrDefault();
                if (data != null)
                {
                    var indexId = data.IndexId as string;
                    var accountId = data.AccountId as string;
                    return new AccountIndex(indexId, accountId, accountName);
                }
                return null;
            }
        }
        public void Add(AccountIndex index)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Insert(index, Constants.AccountIndexTable);
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
