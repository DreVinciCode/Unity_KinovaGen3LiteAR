using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointStatesSubscriber : UnitySubscriber<MessageTypes.Sensor.JointState>
    {
        public JointStatesPublisher jointStatesPublisher;

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
