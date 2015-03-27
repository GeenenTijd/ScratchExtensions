using System;
using Microsoft.Kinect;
using System.Timers;
using System.Text;

namespace GeenenKinect
{
    public class KinectController
    {
        private static readonly object m_object = new object();

        private KinectSensor kinectSensor = null;
        private CoordinateMapper coordinateMapper = null;
        private BodyFrameReader reader = null;
        private Body[] bodies = null;

        private String m_bodyInfo = null;

        private const int multiplier = 200;

        public KinectController()
        {
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // get the coordinate mapper
                this.coordinateMapper = this.kinectSensor.CoordinateMapper;

                // open the sensor
                this.kinectSensor.Open();

                Console.WriteLine("Kinect Started.");

                // open the reader for the body frames
                this.reader = this.kinectSensor.BodyFrameSource.OpenReader();
                this.reader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        public string getBodyInfo()
        {
            lock (m_object)
            {
                return m_bodyInfo;
            }
        }

        public void Close()
        {
            if (this.reader != null)
            {
                this.reader.Dispose();
                this.reader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            BodyFrameReference frameReference = e.FrameReference;
            try
            {
                BodyFrame frame = frameReference.AcquireFrame();
                if (frame != null)
                {
                    // BodyFrame is IDisposable
                    using (frame)
                    {
                        if (this.bodies == null)
                        {
                            this.bodies = new Body[frame.BodyCount];
                        }

                        frame.GetAndRefreshBodyData(this.bodies);

                        Body closestBody = null;
                        float distance = 9999;

                        foreach (Body body in this.bodies)
                        {
                            if (body.IsTracked)
                            {
                                CameraSpacePoint pos = body.Joints[JointType.SpineShoulder].Position;
                                if (pos.Z < 2.5 && (pos.X > -0.5 && pos.X < 0.5) && pos.Z < distance)
                                {
                                    distance = pos.Z;
                                    closestBody = body;
                                }
                            }
                        }

                        if(closestBody != null)
                        {
                            updateKinect(closestBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void updateKinect(Body body)
        {
            StringBuilder builder = new StringBuilder();

            CameraSpacePoint point = body.Joints[JointType.HandLeft].Position;

            builder.Append("leftHandX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("leftHandY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.HandRight].Position;

            builder.Append("rightHandX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("rightHandY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.ElbowLeft].Position;

            builder.Append("leftElbowX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("leftElbowY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.ElbowRight].Position;

            builder.Append("rightElbowX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("rightElbowY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.Head].Position;

            builder.Append("headX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("headY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.SpineShoulder].Position;

            builder.Append("spineShoulderX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("spineShoulderY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.SpineBase].Position;

            builder.Append("spineBaseX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("spineBaseY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.KneeLeft].Position;

            builder.Append("leftKneeX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("leftKneeY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.KneeRight].Position;

            builder.Append("rightKneeX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("rightKneeY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.AnkleLeft].Position;

            builder.Append("leftFootX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("leftFootY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            point = body.Joints[JointType.AnkleRight].Position;

            builder.Append("rightFootX ");
            builder.AppendLine(Math.Round(point.X * multiplier).ToString());
            builder.Append("rightFootY ");
            builder.AppendLine(Math.Round(point.Y * multiplier).ToString());

            lock (m_object)
            {
                m_bodyInfo = builder.ToString();
            }
        }
    }
}
