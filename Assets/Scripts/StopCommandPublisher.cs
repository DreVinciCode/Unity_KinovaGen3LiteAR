using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class StopCommandPublisher : UnityPublisher<MessageTypes.Std.Empty>
    {

        private MessageTypes.Std.Empty _stopMessage;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            _stopMessage = new MessageTypes.Std.Empty();
        }

        public void CallStop()
        {
            Publish(_stopMessage);
        }
    }
}
