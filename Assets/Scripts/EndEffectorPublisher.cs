using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class EndEffectorPublisher : MonoBehaviour
    {
        public GameObject EndEffector_Marker;
        public GameObject TargetPoseObject;

        private bool isMessageReceived;
        private MessageTypes.Geometry.Twist _message;
        private MessageTypes.Geometry.Pose _pose;

        private float _linearX;
        private float _linearY;
        private float _linearZ;
        private float _angularX;
        private float _angularY;
        private float _angularZ;
        private Vector3 _offset = new Vector3(0, 0f, 0);

        public void Write(MessageTypes.Geometry.Twist message)
        {
            _linearX = (float)message.linear.x;
            _linearY = (float)message.linear.y;
            _linearZ = (float)message.linear.z;

            _angularX = (float)message.angular.x;
            _angularY = (float)message.angular.y;
            _angularZ = (float)message.angular.z;

            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            var pose = new MessageTypes.Geometry.Pose();

            pose.position.x = _linearX;
            pose.position.y = _linearY;
            pose.position.z = _linearZ;

            var orientation = Quaternion.Euler(_angularX,_angularY, _angularZ);

            pose.orientation.x = orientation.x;
            pose.orientation.y = orientation.y;
            pose.orientation.z = orientation.z;
            pose.orientation.w = orientation.w;

            _pose = pose;

            EndEffector_Marker.transform.localPosition = GetPosition(pose).Ros2Unity();
            EndEffector_Marker.transform.rotation = GetRotation(pose).Ros2Unity();

            isMessageReceived = false;
        }

        public void ResetTargetPoseObject()
        {
            TargetPoseObject.transform.localPosition = GetPosition(_pose).Ros2Unity() + _offset;
            TargetPoseObject.transform.localRotation = GetRotation(_pose).Ros2Unity();
        }

        private void Update()
        {
            if (isMessageReceived)
            {
                ProcessMessage();
            }
        }

        private Vector3 GetPosition(MessageTypes.Geometry.Pose message)
        {
            return new Vector3(
                (float)message.position.x,
                (float)message.position.y,
                (float)message.position.z);
        }

        private Quaternion GetRotation(MessageTypes.Geometry.Pose message)
        {
            return new Quaternion(
                (float)message.orientation.x,
                (float)message.orientation.y,
                (float)message.orientation.z,
                (float)message.orientation.w);
        }
    }
}
