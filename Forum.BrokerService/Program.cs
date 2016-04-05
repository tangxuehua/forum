using Topshelf;

namespace Forum.BrokerService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static int Main()
        {
			return (int)HostFactory.Run(x =>
			{
				x.RunAsLocalSystem();
				x.SetServiceName("Forum.BrokerService");
				x.SetDisplayName("Forum BrokerService");
				x.SetDescription("Forum BrokerService Samples");

				x.Service<Bootstrap>();

				x.EnableServiceRecovery(r => { r.RestartService(1); });
			});
		}
    }
}
