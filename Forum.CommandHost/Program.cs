using System;

namespace Forum.CommandHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap.Initialize();
            Bootstrap.Start();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
