using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class DisplayTrajectorySubscriber : UnitySubscriber<MessageTypes.Moveit.DisplayTrajectory>
    {
        public DisplayTrajectoryPublisher displayTrajectoryPublisher;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Moveit.DisplayTrajectory message)
        {
            displayTrajectoryPublisher.Write(message);
        }
    }
}

