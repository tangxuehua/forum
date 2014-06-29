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
        public AccountNameUniquenessValidateResult AddAccountNameIndex(string accountName, string accountId)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Insert(new
                    {
                        AccountName = accountName,
                        AccountId = accountId
                    }, Constants.AccountNameIndexTable);
                    return AccountNameUniquenessValidateResult.Success;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 && ex.Message.Contains(Constants.AccountNameIndexTablePrimaryKey))
                    {
                        return AccountNameUniquenessValidateResult.DuplicateAccountName;
                    }
                    throw;
                }
            }
        }
        public string GetAccountId(string accountName)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<string>(new { AccountName = accountName }, Constants.AccountNameIndexTable, "AccountId").SingleOrDefault();
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
