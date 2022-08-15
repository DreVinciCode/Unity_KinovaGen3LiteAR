using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RosSharp.RosBridgeClient
{
    public class TwistCommandPublisher : UnityPublisher<MessageTypes.KortexDriver.TwistCommand>
    {
        public bool _publishMessageCheck { get; set; }

        private MessageTypes.KortexDriver.TwistCommand message;

        private uint _referenceFrame = 0;
        private uint _duration = 0;

        private bool _activeState = false;


        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            _publishMessageCheck = false;
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.KortexDriver.TwistCommand();
            message.twist = new MessageTypes.KortexDriver.Twist();
            message.reference_frame = _referenceFrame;
            message.duration = _duration;
        }

        private void PublishMessage()
        {
            message.twist.linear_x = 0f;
            message.twist.linear_y = 0f;
            message.twist.linear_z = 0f;

            message.twist.angular_x = 0f;
            message.twist.angular_y = 0f;
            message.twist.angular_z = 0f;

            Publish(message);
        }

        private void FixedUpdate()
        {



            if (_publishMessageCheck)
                PublishMessage();
        }
    }
}
