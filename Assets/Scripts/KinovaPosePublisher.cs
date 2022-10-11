using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class KinovaPosePublisher : UnityPublisher<MessageTypes.Std.Int16>
    {
        public bool _publishMessageCheck { get; set; }

        private MessageTypes.Std.Int16 _message;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            _message = new MessageTypes.Std.Int16();
        }

        public void SendKinovaPosition(int value)
        {
            _message.data = (short)value;
            ProcessMessage();
        }

        private void ProcessMessage()
        {
            if (_publishMessageCheck)
                Publish(_message);
        }
    }
}

