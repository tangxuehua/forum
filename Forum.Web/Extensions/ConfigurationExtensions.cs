using ENode;
using Forum.Domain.Repositories;
using Forum.Repository;

namespace Forum.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Configuration MongoAccountRegistrationInfoRepository(this Configuration configuration, string connectionString, string accountCollectionName)
        {
            configuration.SetDefault<IAccountRegistrationInfoRepository, MongoAccountRegistrationInfoRepository>(
                new MongoAccountRegistrationInfoRepository(connectionString, accountCollectionName));
            return configuration;
        }
    }
}