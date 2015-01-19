using System;
using System.Text;

using GeenenLeap;
using System.IO;

namespace KinectServer
{
    public class LeapHttpServer : HttpServer
    {

        private static LeapController leapController = null;

        public LeapHttpServer(int port): base(port)
        {
            leapController = new LeapController();
            leapController.Start();
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            if (p.http_url.Equals("/poll"))
            {
                p.writeSuccess();

                string response = leapController.getHands();
                Console.WriteLine(response);
                p.outputStream.WriteLine(response);
            }
            else
            {
                p.writeSuccess();
            }
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST");
        }
    }
}
