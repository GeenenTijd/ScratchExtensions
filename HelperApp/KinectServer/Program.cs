using System;
using System.Threading;

namespace KinectServer
{
    class Program
    {
        static void startKinectServer()
        {
            KinectHttpServer httpServer = new KinectHttpServer(3330);
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }

        static void startLeapMotionServer()
        {
            LeapHttpServer httpServer = new LeapHttpServer(3331);
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Please choose a server to start:");
            Console.WriteLine("1: Kinect Server");
            Console.WriteLine("2: Leap Motion Server");

            string read = Console.ReadLine();
            int number = 0;

            while(!int.TryParse(read, out number))
            {
                read = Console.ReadLine();
            }

            if(number == 1)
            {
                startKinectServer();
            }
            else if( number == 2)
            {
                startLeapMotionServer();
            }
        }
    }
}
