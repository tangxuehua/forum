using System;
using System.Collections.Concurrent;
using ECommon.IoC;
using Forum.Domain.Accounts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Forum.Domain.Repositories.MongoDB
{
    [Component]
    public class AccountRegistrationInfoRepository : IAccountRegistrationInfoRepository
    {
        private readonly string _connectionString;
        private readonly string _accountCollectionName;
        private readonly ConcurrentDictionary<string, MongoCollection<BsonDocument>> _collectionDict;

        public AccountRegistrationInfoRepository(string connectionString, string accountCollectionName)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (accountCollectionName == null)
            {
                throw new ArgumentNullException("accountCollectionName");
            }
            _connectionString = connectionString;
            _accountCollectionName = accountCollectionName;
            _collectionDict = new ConcurrentDictionary<string, MongoCollection<BsonDocument>>();
        }

        public void Add(AccountRegistrationInfo info)
        {
            try
            {
                GetAccountCollection().Insert(new BsonDocument {
                    { "_id", info.AccountName },
                    { "AccountId", info.AccountId.ToString() },
                    { "Status", AccountRegistrationStatus.Created.ToString() }
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
        public void Update(AccountRegistrationInfo info)
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
        public AccountRegistrationInfo GetByAccountName(string accountName)
        {
            var document = GetAccountCollection().FindOneById(new BsonString(accountName));
            if (document != null)
            {
                return new AccountRegistrationInfo(
                    document["AccountId"].AsString,
                    document["_id"].AsString,
                    (AccountRegistrationStatus)Enum.Parse(typeof(AccountRegistrationStatus), document["Status"].AsString));
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
