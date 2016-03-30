using Topshelf;

namespace Forum.EventService
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
				x.SetServiceName("Forum.CommandService");
				x.SetDisplayName("Forum CommandService");
				x.SetDescription("Forum.CommandService Samples");

				x.Service<Bootstrap>();

				x.EnableServiceRecovery(r => { r.RestartService(1); });
			});
		}
    }
}
