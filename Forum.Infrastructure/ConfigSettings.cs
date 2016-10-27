using System.Configuration;

namespace Forum.Infrastructure
{
    public class ConfigSettings
    {
        public static string ForumConnectionString { get; set; }
        public static string ENodeConnectionString { get; set; }
        public static int NameServerPort { get; set; }
        public static int BrokerProducerPort { get; set; }
        public static int BrokerConsumerPort { get; set; }
        public static int BrokerAdminPort { get; set; }

        public static void Initialize()
        {
            if (ConfigurationManager.ConnectionStrings["forum"] != null)
            {
                ForumConnectionString = ConfigurationManager.ConnectionStrings["forum"].ConnectionString;
            }

            if (ConfigurationManager.ConnectionStrings["enode"] != null)
            {
                ENodeConnectionString = ConfigurationManager.ConnectionStrings["enode"].ConnectionString;
            }

            NameServerPort = 10191;
            BrokerProducerPort = 10192;
            BrokerConsumerPort = 10193;
            BrokerAdminPort = 10194;
        }
    }
}
