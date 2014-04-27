using System;
using System.Data.SqlClient;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Domain.Repositories.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class RegistrationRepository : IRegistrationRepository
    {
        public void Add(Registration registration)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Insert(new
                    {
                        AccountId = registration.AccountId,
                        AccountName = registration.AccountName,
                        Status = registration.Status
                    }, Constants.RegistrationTable);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        if (ex.Message.Contains(Constants.RegistrationTablePrimaryKeyName))
                        {
                            throw new DuplicateAccountNameException(registration.AccountName, ex);
                        }
                    }
                    throw;
                }
            }
        }
        public void Update(Registration registration)
        {
            using (var connection = GetConnection())
            {
                connection.Update(new { Status = registration.Status }, new { AccountId = registration.AccountId }, Constants.RegistrationTable);
            }
        }
        public Registration GetByAccountName(string accountName)
        {
            using (var connection = GetConnection())
            {
                var data = connection.QueryList(new { AccountName = accountName }, Constants.RegistrationTable).SingleOrDefault();
                if (data != null)
                {
                    var status = (RegistrationStatus)Enum.Parse(typeof(RegistrationStatus), data.Status);
                    return new Registration(data.AccountId, accountName, status);
                }
            }
            return null;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
