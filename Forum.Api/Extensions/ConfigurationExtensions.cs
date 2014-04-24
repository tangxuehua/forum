using ENode.Configurations;
using Forum.Domain.Accounts;
using Forum.Domain.Repositories.MongoDB;

namespace Forum.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ENodeConfiguration MongoAccountRegistrationInfoRepository(this ENodeConfiguration configuration, string connectionString, string accountCollectionName)
        {
            configuration.GetCommonConfiguration().SetDefault<IAccountRegistrationInfoRepository, AccountRegistrationInfoRepository>(
                new AccountRegistrationInfoRepository(connectionString, accountCollectionName));
            return configuration;
        }
    }
}