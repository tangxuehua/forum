using System;
using System.Collections.Concurrent;
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

        public void Add(Registration info)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Insert(
                    new
                    {
                        AccountId = info.AccountId,
                        AccountName = info.AccountName,
                        Status = RegistrationStatus.Created.ToString()
                    }, "tb_Registration");
            }

            try
            {
                GetAccountCollection().Insert(new BsonDocument {
                    { "_id", info.AccountName },
                    { "AccountId", info.AccountId.ToString() },
                    { "Status", RegistrationStatus.Created.ToString() }
                });
            }
            catch (WriteConcernException ex)
            {
                if (ex.CommandResult != null && ex.CommandResult.Code != null && ex.CommandResult.Code.Value == 11000)
                {
                    throw new DuplicateAccountNameException(info.AccountName, ex);
                }
            }
        }
        public void Update(Registration info)
        {
            var accountCollection = GetAccountCollection();
            var document = accountCollection.FindOneById(new BsonString(info.AccountName));
            if (document == null)
            {
                throw new Exception(string.Format("无法根据账号：{0}获取账号注册信息。", info.AccountName));
            }
            document["Status"] = info.RegistrationStatus.ToString();
            accountCollection.Save(document);
        }
        public Registration GetByAccountName(string accountName)
        {
            var document = GetAccountCollection().FindOneById(new BsonString(accountName));
            if (document != null)
            {
                return new Registration(
                    document["AccountId"].AsString,
                    document["_id"].AsString,
                    (RegistrationStatus)Enum.Parse(typeof(RegistrationStatus), document["Status"].AsString));
            }
            return null;
        }

        private MongoCollection<BsonDocument> GetAccountCollection()
        {
            MongoCollection<BsonDocument> collection;
            if (_collectionDict.TryGetValue(_accountCollectionName, out collection)) return collection;

            lock (this)
            {
                collection = new MongoClient(_connectionString).GetServer().GetDatabase(new MongoUrl(_connectionString).DatabaseName).GetCollection(_accountCollectionName);
                _collectionDict.TryAdd(_accountCollectionName, collection);
            }

            return collection;
        }
    }
}
