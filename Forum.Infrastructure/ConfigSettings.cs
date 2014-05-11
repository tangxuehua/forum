using System.Configuration;

namespace Forum.Infrastructure
{
    public class ConfigSettings
    {
        public static string ConnectionString { get; set; }

        public static void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }
    }
}
