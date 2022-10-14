using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TargetEndPosePublisher : UnityPublisher<MessageTypes.Geometry.PoseStamped>
    {

        private MessageTypes.Geometry.PoseStamped _message;

        private Vector3 _position;
        private Quaternion _orientation;
        public GameObject TargetPositionObject;


        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            _message = new MessageTypes.Geometry.PoseStamped();
        }

        public void UpdateTargetPoseMessage()
        {
            _position = TargetPositionObject.transform.localPosition.Unity2Ros();
            _orientation = TargetPositionObject.transform.localRotation.Unity2Ros();

            _message.pose.position.x = _position.x;
            _message.pose.position.y = _position.y;
            _message.pose.position.z = _position.z;

            _message.pose.orientation.x = _orientation.x;
            _message.pose.orientation.y = _orientation.y;
            _message.pose.orientation.z = _orientation.z;
            _message.pose.orientation.w = _orientation.w;

            Publish(_message);
        }

        public void PublishTargetPose()
        {
             Publish(_message);
        } 
    }
}
