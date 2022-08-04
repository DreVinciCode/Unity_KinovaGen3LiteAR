using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointStateSubscriber_Kinova_gen3lite : UnitySubscriber<MessageTypes.Sensor.JointState>
    {
        public JointStatePublisher_Kinova_gen3Lite jointStatesPublisher;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
        {
            jointStatesPublisher.Write(message);
        }
    }
}
