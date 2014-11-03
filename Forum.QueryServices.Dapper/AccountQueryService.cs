using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Infrastructure;
using Forum.QueryServices.DTOs;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class AccountQueryService : BaseQueryService, IAccountQueryService
    {
        public AccountInfo Find(string name)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<AccountInfo>(new { Name = name }, Constants.AccountTable).SingleOrDefault();
            }
        }
    }
}
