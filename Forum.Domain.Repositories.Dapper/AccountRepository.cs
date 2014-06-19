using System.Data.SqlClient;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using ECommon.Utilities;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Domain.Repositories.Dapper
{
    //TODO
    //[Component(LifeStyle.Singleton)]
    //public class AccountRepository : IAccountRepository
    //{
    //    public void Add(Registration account)
    //    {
    //        using (var connection = GetConnection())
    //        {
    //            try
    //            {
    //                connection.Insert(new
    //                {
    //                    AccountId = account.AccountId,
    //                    AccountName = account.AccountName
    //                }, Constants.AccountInfoTable);
    //            }
    //            catch (SqlException ex)
    //            {
    //                if (ex.Number == 2627)
    //                {
    //                    if (ex.Message.Contains(Constants.AccountInfoTablePrimaryKeyName))
    //                    {
    //                        throw new DuplicateAccountNameException(account.AccountName, ex);
    //                    }
    //                }
    //                throw;
    //            }
    //        }
    //    }
    //    public Registration Find(string name)
    //    {
    //        using (var connection = GetConnection())
    //        {
    //            var data = connection.QueryList(new { AccountName = name }, Constants.AccountInfoTable).SingleOrDefault();
    //            if (data != null)
    //            {
    //                return new Registration(data.AccountId, name);
    //            }
    //        }
    //        return null;
    //    }
    //    public string GetNextAccountId()
    //    {
    //        return ObjectId.GenerateNewStringId();
    //    }

    //    private SqlConnection GetConnection()
    //    {
    //        return new SqlConnection(ConfigSettings.ConnectionString);
    //    }
    //}
}
