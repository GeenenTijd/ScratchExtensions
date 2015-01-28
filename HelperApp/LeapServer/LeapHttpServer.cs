using System;
using System.Text;

using GeenenLeap;
using System.IO;
using LeapServer;
using SimpleHttpServer;

namespace LeapServer
{
    public class LeapHttpServer : HttpServer
    {

        private static LeapController leapController = null;

        public LeapHttpServer(int port)
            : base(port)
        {
            leapController = new LeapController();
            leapController.Start();
        }

        public void stop()
        {
            leapController.Stop();
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            if (p.http_url.Equals("/poll"))
            {
                p.writeSuccess();
                string response = leapController.getHands();
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
