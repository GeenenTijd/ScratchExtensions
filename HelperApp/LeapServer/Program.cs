using System;
using System.Threading;

namespace LeapServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Leap Motion Controller.");

            LeapHttpServer httpServer = new LeapHttpServer(3331);
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }
    }
}
