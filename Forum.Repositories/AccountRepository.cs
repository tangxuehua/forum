using System;
using System.Collections.Concurrent;
using ENode.Domain;
using ENode.Infrastructure;
using Forum.Domain;
using Forum.Domain.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Forum.Repository {
    [Component]
    public class AccountRepository : IAccountRepository {
        private string _connectionString = "mongodb://localhost/ForumDB";
        private string _accountCollection = "AccountCollection";
        private ConcurrentDictionary<string, MongoCollection<BsonDocument>> _collectionDict = new ConcurrentDictionary<string, MongoCollection<BsonDocument>>();
        private IRepository _repository;

        public AccountRepository(IRepository repository) {
            _repository = repository;
        }

        public void AddAccount(Guid accountId, string name) {
            try {
                GetAccountCollection().Insert(new BsonDocument {
                    { "_id", accountId.ToString() },
                    { "Name", name },
                    { "Status", 1 }
                });
            }
            catch (WriteConcernException ex) {
                if (ex.CommandResult != null && ex.CommandResult.Code != null && ex.CommandResult.Code.Value == 11000) {
                    throw new DuplicateAccountNameException(name, ex);
                }
            }
        }
        public void ConfirmAccount(Guid accountId) {
            var accountCollection = GetAccountCollection();
            var document = accountCollection.FindOneById(new BsonString(accountId.ToString()));
            document["Status"] = 2;
            accountCollection.Save(document);
        }
        public Account GetAccount(string name) {
            var document = GetAccountCollection().FindOne(Query.EQ("Name", name));
            if (document != null) {
                var accountId = document["_id"].AsString;
                return _repository.Get<Account>(accountId);
            }
            return null;
        }

        private MongoCollection<BsonDocument> GetAccountCollection() {
            MongoCollection<BsonDocument> collection;

            if (!_collectionDict.TryGetValue(_accountCollection, out collection)) {
                lock (this) {
                    var client = new MongoClient(_connectionString);
                    var database = client.GetServer().GetDatabase(new MongoUrl(_connectionString).DatabaseName);
                    collection = database.GetCollection(_accountCollection);
                    collection.EnsureIndex(IndexKeys.Ascending("Name"), IndexOptions.SetName("AccountNameUniqueIndex").SetUnique(true));
                    _collectionDict.TryAdd(_accountCollection, collection);
                }
            }

            return collection;
        }
    }
}
