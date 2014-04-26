using ENode.Configurations;
using Forum.Domain.Accounts;
using Forum.Domain.Repositories.Dapper;

namespace Forum.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ENodeConfiguration SetRegistrationDapperRepository(this ENodeConfiguration configuration, string connectionString)
        {
            configuration.GetCommonConfiguration().SetDefault<IRegistrationRepository, RegistrationRepository>(new RegistrationRepository(connectionString));
            return configuration;
        }
    }
}