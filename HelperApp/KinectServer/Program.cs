﻿using System;
using System.Threading;

namespace KinectServer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting Kinect Controller.");

            KinectHttpServer httpServer = new KinectHttpServer(3330);
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }
    }
}
