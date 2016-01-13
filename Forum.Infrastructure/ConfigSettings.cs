using System.Configuration;

namespace Forum.Infrastructure
{
    public class ConfigSettings
    {
        public static string ForumConnectionString { get; set; }
        public static string ENodeConnectionString { get; set; }

        public static void Initialize()
        {
            ForumConnectionString = ConfigurationManager.ConnectionStrings["forum"].ConnectionString;
            ENodeConnectionString = ConfigurationManager.ConnectionStrings["enode"].ConnectionString;
        }
    }
}
