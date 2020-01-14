using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Domain.Dapper
{
    [Component]
    public class AccountIndexRepository : IAccountIndexRepository
    {
        public async Task<AccountIndex> FindByAccountName(string accountName)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var data = await connection.QueryListAsync(new { AccountName = accountName }, Constants.AccountIndexTable);
                if (data.SingleOrDefault() != null)
                {
                    return new AccountIndex(data.SingleOrDefault().AccountId as string, accountName);
                }
                return null;
            }
        }
        public async Task Add(AccountIndex index)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                await connection.InsertAsync(new
                {
                    AccountId = index.AccountId,
                    AccountName = index.AccountName
                }, Constants.AccountIndexTable);
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ForumConnectionString);
        }
    }
}
