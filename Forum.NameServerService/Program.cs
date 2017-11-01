using System;

namespace Forum.NameServerService
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap.Initialize();
            Bootstrap.Start();
            Console.WriteLine("Press enter to exit...");
            var line = Console.ReadLine();
            while (line != "exit")
            {
                switch (line)
                {
                    case "cls":
                        Console.Clear();
                        break;
                    default:
                        return;
                }
                line = Console.ReadLine();
            }
        }
    }
}
