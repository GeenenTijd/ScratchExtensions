using System;
using System.IO;
using System.Text;
using GeenenKinect;
using SimpleHttpServer;

namespace KinectServer
{
    public class KinectHttpServer : HttpServer
    {

        private static KinectController kinectController = null;

        public KinectHttpServer(int port) : base(port)
        {
            kinectController = new KinectController();
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            if (p.http_url.Equals("/poll"))
            {
                p.writeSuccess();
                p.outputStream.WriteLine(kinectController.getBodyInfo());
            }
            else
            {
                p.writeSuccess();
            }
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.http_url);
        }
    }
}
