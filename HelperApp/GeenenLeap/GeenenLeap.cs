using System;

using Leap;
using System.Text;

namespace GeenenLeap
{
    public class LeapController
    {
        private Controller m_controller;
        private LeapListener m_listener;

        public void Start()
        {
            try
            {
                m_controller = new Controller();
                m_listener = new LeapListener();

                m_controller.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);
                m_controller.AddListener(m_listener);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public void Stop()
        {
            m_controller.RemoveListener(m_listener);
            m_controller.Dispose();
        }

        public string getHands()
        {
            return m_listener.getHands();
        }
    }

    public class LeapListener : Listener
    {

        private static readonly object m_object = new object();

        private string m_handInfo;

        public string getHands()
        {
            lock (m_object)
            {
                return m_handInfo;
            }
        }
        
        public override void OnInit(Controller controller)
        {
            Console.WriteLine("Leap Motion Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            Console.WriteLine("Leap Motion Connected");
        }

        public override void OnDisconnect(Controller controller)
        {
            Console.WriteLine("Leap Motion Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            Console.WriteLine("Leap Motion Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            StringBuilder builder = new StringBuilder();

            for (int j = 0; j < 2; ++j)
            {
                Hand hand = frame.Hands[j];
                if (hand.IsValid)
                {
                    builder.Append("hand-" + (j + 1) + "-x ");
                    builder.AppendLine(hand.PalmPosition.x.ToString());

                    builder.Append("hand-" + (j + 1) + "-y ");
                    builder.AppendLine(((hand.PalmPosition.y - 220) * 1.6f).ToString());

                    builder.Append("hand-" + (j + 1) + "-z ");
                    builder.AppendLine(hand.PalmPosition.z.ToString());

                    builder.Append("hand-" + (j + 1) + "-open ");
                    builder.AppendLine(hand.Fingers.Count >= 3 ? "1" : "0");

                    builder.Append("hand-" + (j + 1) + "-visible ");
                    builder.AppendLine("1");
                }
                else
                {
                    builder.AppendLine("hand-" + (j + 1) + "-x 0");
                    builder.AppendLine("hand-" + (j + 1) + "-y 0");
                    builder.AppendLine("hand-" + (j + 1) + "-z 0");
                    builder.AppendLine("hand-" + (j + 1) + "-open 0");
                    builder.AppendLine("hand-" + (j + 1) + "-visible 0");
                }
            }

            lock (m_object)
            {
                m_handInfo = builder.ToString();
            }            
        }
    }
}
