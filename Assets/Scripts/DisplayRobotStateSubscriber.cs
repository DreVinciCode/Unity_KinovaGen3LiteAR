using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class DisplayRobotStateSubscriber : UnitySubscriber<MessageTypes.Moveit.DisplayRobotState>
    {
        public DisplayRobotStatePublisher displayRobotStatePublisher;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Moveit.DisplayRobotState message)
        {
            displayRobotStatePublisher.Write(message);
        }

    }
}

