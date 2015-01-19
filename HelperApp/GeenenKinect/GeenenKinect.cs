using System;
using Microsoft.Kinect;
using System.Timers;

namespace GeenenKinect
{
    public struct BodyInfo
    {
        public float leftHandX;
        public float leftHandY;

        public float rightHandX;
        public float rightHandY;

        public float leftElbowX;
        public float leftElbowY;

        public float rightElbowX;
        public float rightElbowY;

        public float headX;
        public float headY;

        public float spineShoulderX;
        public float spineShoulderY;

        public float spineBaseX;
        public float spineBaseY;

        public float leftKneeX;
        public float leftKneeY;

        public float rightKneeX;
        public float rightKneeY;

        public float leftFootX;
        public float leftFootY;

        public float rightFootX;
        public float rightFootY;

    }

    public class KinectController
    {
        private KinectSensor kinectSensor = null;
        private CoordinateMapper coordinateMapper = null;
        private BodyFrameReader reader = null;
        private Body[] bodies = null;

        private BodyInfo lastInfo;

        private const int multiplier = 100 * 3;

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
            CameraSpacePoint point = body.Joints[JointType.HandLeft].Position;
            lastInfo.leftHandX = point.X * multiplier;
            lastInfo.leftHandY = point.Y * multiplier;

            point = body.Joints[JointType.HandRight].Position;
            lastInfo.rightHandX = point.X * multiplier;
            lastInfo.rightHandY = point.Y * multiplier;

            point = body.Joints[JointType.ElbowLeft].Position;
            lastInfo.leftElbowX = point.X * multiplier;
            lastInfo.leftElbowY = point.Y * multiplier;

            point = body.Joints[JointType.ElbowRight].Position;
            lastInfo.rightElbowX = point.X * multiplier;
            lastInfo.rightElbowY = point.Y * multiplier;

            point = body.Joints[JointType.Head].Position;
            lastInfo.headX = point.X * multiplier;
            lastInfo.headY = point.Y * multiplier;

            point = body.Joints[JointType.SpineShoulder].Position;
            lastInfo.spineShoulderX = point.X * multiplier;
            lastInfo.spineShoulderY = point.Y * multiplier;

            point = body.Joints[JointType.SpineBase].Position;
            lastInfo.spineBaseX = point.X * multiplier;
            lastInfo.spineBaseY = point.Y * multiplier;

            point = body.Joints[JointType.KneeLeft].Position;
            lastInfo.leftKneeX = point.X * multiplier;
            lastInfo.leftKneeY = point.Y * multiplier;

            point = body.Joints[JointType.KneeRight].Position;
            lastInfo.rightKneeX = point.X * multiplier;
            lastInfo.rightKneeY = point.Y * multiplier;

            point = body.Joints[JointType.FootLeft].Position;
            lastInfo.leftFootX = point.X * multiplier;
            lastInfo.leftFootY = point.Y * multiplier;

            point = body.Joints[JointType.FootRight].Position;
            lastInfo.rightFootX = point.X * multiplier;
            lastInfo.rightFootY = point.Y * multiplier;
        }

        public BodyInfo getBodyInfo()
        {
            return lastInfo;
        }
    }
}
