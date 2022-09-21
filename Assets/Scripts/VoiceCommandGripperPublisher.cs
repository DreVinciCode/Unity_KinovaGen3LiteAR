using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class VoiceCommandGripperPublisher : UnityPublisher<MessageTypes.Std.Float32>
    {
        private MessageTypes.Std.Float32 message;
        public bool _publishMessageCheck { get; set; }

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Std.Float32();
        }

        private void PublishMessage()
        {
            if(_publishMessageCheck)
                Publish(message);
        }

        public void CloseGripper()
        {
            message.data = 0;
            PublishMessage();
        }

        public void OpenGripper()
        {
            message.data = 1;
            PublishMessage();
        }

        public void GripperPose(float value)
        {
            if (value >= 0 && value <= 1)
                message.data = value;
            else
                message.data = -1;

            PublishMessage();
        }
    }
}
