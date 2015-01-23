using System;
using System.ServiceProcess;

namespace Forum.BrokerService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                ServiceBase.Run(new Service1());
            }
            else
            {
                Bootstrap.Initialize();
                Bootstrap.Start();
                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
