using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointTrajectoryControllerStateSubscriber : UnitySubscriber<MessageTypes.Control.JointTrajectoryControllerState>
    {
        public JointTrajectoryControllerStatePublisher jointTrajectoryControllerStatePublisher;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Control.JointTrajectoryControllerState message)
        {
            jointTrajectoryControllerStatePublisher.Write(message);
        }
    }
}
