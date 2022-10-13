using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TwistEndEffectorSubscriber : UnitySubscriber<MessageTypes.Geometry.Twist>
    {
        public EndEffectorPublisher endEffectorPublisher;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.Twist message)
        {
            endEffectorPublisher.Write(message);
        }
    }
}
