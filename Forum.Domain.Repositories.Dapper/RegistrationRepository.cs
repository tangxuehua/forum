using System;
using System.Data.SqlClient;
using ECommon.IoC;
using ENode.Infrastructure.Dapper;
using Forum.Domain.Accounts;

namespace Forum.Domain.Repositories.Dapper
{
    [Component]
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly string _connectionString;

        public RegistrationRepository(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }
            _connectionString = connectionString;
        }

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
                    }, "tb_Registration");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601)
                    {
                        if (ex.Message.Contains("PK_tb_Registration"))
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
                connection.Update(new { Status = registration.Status }, new { AccountId = registration.AccountId }, "tb_Registration");
            }
        }
        public Registration GetByAccountName(string accountName)
        {
            using (var connection = GetConnection())
            {
                var data = connection.QuerySingleOrDefault(new { AccountName = accountName }, "tb_Registration");
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
            return new SqlConnection(_connectionString);
        }
    }
}
