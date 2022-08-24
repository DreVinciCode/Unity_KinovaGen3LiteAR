using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class GipperPosePublisher : UnityPublisher<MessageTypes.Std.Float32>
    {
        private float _indexThumb_maxDistance = 0.001f;
        private float _gripperPosition = 0;

        private MessageTypes.Std.Float32 message;

        private bool _publishedMessage = true;

        public bool _publishMessageCheck { get; set; }

        private MixedRealityPose _leftThumbPose, _leftIndexPose;

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
            StartCoroutine(WaitForSeconds());
        }

        private void FixedUpdate()
        {
            if(HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out _leftIndexPose) &&
                    HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Left, out _leftThumbPose))
            {
                _gripperPosition = Vector3.Distance(_leftIndexPose.Position, _leftThumbPose.Position);

                if (_gripperPosition > _indexThumb_maxDistance)
                    _indexThumb_maxDistance = _gripperPosition;

                var adjustedValue = ((-1 / _indexThumb_maxDistance) * _gripperPosition) + 1;

                if (adjustedValue > 0 && adjustedValue < 1)
                {
                    message.data = adjustedValue;
                }
                else if (adjustedValue > 1)
                {
                    message.data = 1;
                }
                else if (adjustedValue < 0)
                {
                    message.data = 0;
                }
                else
                    message.data = -1;
       
            }
            else
                message.data = -1f;

            if (_publishMessageCheck && _publishedMessage)
            {
                PublishMessage();
                _publishedMessage = false;
            }

        }

        private IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(1f);
            Publish(message);
            _publishedMessage = true;
        }
    }
}