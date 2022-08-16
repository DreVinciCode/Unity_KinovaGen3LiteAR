using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class EmergencyStopCommandPublisher : UnityPublisher<MessageTypes.Std.Empty>
    {

        private MessageTypes.Std.Empty _EmergencyStopMessage;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            _EmergencyStopMessage = new MessageTypes.Std.Empty();
        }

        public void CallEmergencyStop()
        {
            Publish(_EmergencyStopMessage);
        }
    }
}
