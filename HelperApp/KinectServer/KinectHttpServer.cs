using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeenenKinect;

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
            Console.WriteLine("request: {0}", p.http_url);
            if (p.http_url.Equals("/poll"))
            {
                p.writeSuccess();
                p.outputStream.WriteLine(getResponse());
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

        private string getResponse()
        {
            BodyInfo body = kinectController.getBodyInfo();

            StringBuilder builder = new StringBuilder();

            builder.Append("leftHandX ");
            builder.AppendLine(body.leftHandX.ToString());
            builder.Append("leftHandY ");
            builder.AppendLine(body.leftHandY.ToString());

            builder.Append("rightHandX ");
            builder.AppendLine(body.rightHandX.ToString());
            builder.Append("rightHandY ");
            builder.AppendLine(body.rightHandY.ToString());

            builder.Append("leftElbowX ");
            builder.AppendLine(body.leftElbowX.ToString());
            builder.Append("leftElbowY ");
            builder.AppendLine(body.leftElbowY.ToString());

            builder.Append("rightElbowX ");
            builder.AppendLine(body.rightElbowX.ToString());
            builder.Append("rightElbowY ");
            builder.AppendLine(body.rightElbowY.ToString());

            builder.Append("headX ");
            builder.AppendLine(body.headX.ToString());
            builder.Append("headY ");
            builder.AppendLine(body.headY.ToString());

            builder.Append("spineShoulderX ");
            builder.AppendLine(body.spineShoulderX.ToString());
            builder.Append("spineShoulderY ");
            builder.AppendLine(body.spineShoulderY.ToString());

            builder.Append("spineBaseX ");
            builder.AppendLine(body.spineBaseX.ToString());
            builder.Append("spineBaseY ");
            builder.AppendLine(body.spineBaseY.ToString());

            builder.Append("leftKneeX ");
            builder.AppendLine(body.leftKneeX.ToString());
            builder.Append("leftKneeY ");
            builder.AppendLine(body.leftKneeY.ToString());

            builder.Append("rightKneeX ");
            builder.AppendLine(body.rightKneeX.ToString());
            builder.Append("rightKneeY ");
            builder.AppendLine(body.rightKneeY.ToString());

            builder.Append("leftFootX ");
            builder.AppendLine(body.leftFootX.ToString());
            builder.Append("leftFootY ");
            builder.AppendLine(body.leftFootY.ToString());

            builder.Append("rightFootX ");
            builder.AppendLine(body.rightKneeX.ToString());
            builder.Append("rightFootY ");
            builder.AppendLine(body.rightFootY.ToString());

            return builder.ToString();
        }

    }
}
