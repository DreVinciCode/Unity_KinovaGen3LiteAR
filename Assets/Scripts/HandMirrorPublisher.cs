using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class HandMirrorPublisher : UnityPublisher<MessageTypes.Geometry.Twist>
    {
        public GameObject LeftHandIndexMarker;
        public GameObject Target_EndEffector;
        public TMP_Text Status;
        public TMP_Text Direction;

        public bool _publishMessageCheck { get; set; }
        public bool _objectDetected { get; set; }

        private MessageTypes.Geometry.Twist message;
        private float _linearX = 0;
        private float _linearY = 0;
        private float _linearZ = 0;
        private float _distanceThreshold = 0.01f;
        private float _forwardSeparation = 0.1f;
        private float left_thumbCurl;
        private float left_indexCurl;
        private float left_middleCurl;
        private float left_ringCurl;
        private float left_pinkyCurl;

        private Vector3 _indexOffset = new Vector3(0, 0, 0.2f);

        private bool _activeState = false;
        private bool _stopChecked = false;

        private MixedRealityPose _leftThumbPose, _leftPalmPose, _leftIndexPose, _leftMiddlePose, _rightThumbPose, _rightIndexPose;

        GameObject _leftIndexObject;


        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            Status.text = "Inactive";
            _leftIndexObject = Instantiate(LeftHandIndexMarker, Camera.main.transform);

        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Twist();
        }

        private void PublishMessage()
        {
            Vector3 linearVelocity = new Vector3(_linearX, _linearY, _linearZ);
            Vector3 angularVelocity = new Vector3(0f, 0f, 0f);

            message.linear = GetGeometryVector3(linearVelocity);
            message.angular = GetGeometryVector3(angularVelocity);

            Publish(message);
        }

        private void Update()
        {
            left_thumbCurl = HandPoseUtils.ThumbFingerCurl(Handedness.Left);
            left_indexCurl = HandPoseUtils.IndexFingerCurl(Handedness.Left);
            left_middleCurl = HandPoseUtils.MiddleFingerCurl(Handedness.Left);
            left_ringCurl = HandPoseUtils.RingFingerCurl(Handedness.Left);
            left_pinkyCurl = HandPoseUtils.PinkyFingerCurl(Handedness.Left);

            _leftIndexObject.GetComponent<Renderer>().enabled = false;


            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out _rightIndexPose) &&
                HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Right, out _rightThumbPose))
            {
                if (Vector3.Distance(_rightIndexPose.Position, _rightThumbPose.Position) < 0.02f)
                {
                    _activeState = true;
                    Status.text = "Active";
                }
                else
                {
                    Status.text = "Active";
                    _activeState = false;               
                }
            }
            else
            {
                _activeState = false;
                Status.text = "Active";
            }

            if (!_activeState || !_objectDetected)
            {
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                Direction.text = "--";
            }
            else if (left_indexCurl < 0.1f && left_middleCurl < 0.1f && left_ringCurl < 0.1f && left_pinkyCurl < 0.1f && left_thumbCurl < 0.4f)
            {
                //Call the stop command?
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                Direction.text = "Stop";
                _stopChecked = true;
            }
            else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out _leftPalmPose) &&
                HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out _leftIndexPose))
            {
                _leftIndexObject.transform.position = _leftIndexPose.Position + _indexOffset;
                //_leftIndexObject.GetComponent<Renderer>().enabled = true;


                //Match the height here. Kinova arm up/down is Z, Unity is Y
                var z_arm_difference = _leftPalmPose.Position.y - Target_EndEffector.transform.position.y;
                var y_arm_difference = _leftIndexPose.Position.x - Target_EndEffector.transform.position.x;
                var x_arm_difference = _leftIndexPose.Position.z - Target_EndEffector.transform.position.z;

                if (Mathf.Abs( z_arm_difference) > _distanceThreshold)
                {
                    _linearZ = z_arm_difference;
                    _linearX = 0;
                    _linearY = y_arm_difference;
                    Direction.text = (Mathf.Round((z_arm_difference) * 100f) / 100f).ToString();
                }
            }
            else
            {
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                Direction.text = "--";
            }

            if (_publishMessageCheck)
                PublishMessage();
        }

        private static MessageTypes.Geometry.Vector3 GetGeometryVector3(Vector3 vector3)
        {
            MessageTypes.Geometry.Vector3 geometryVector3 = new MessageTypes.Geometry.Vector3();
            geometryVector3.x = vector3.x;
            geometryVector3.y = vector3.y;
            geometryVector3.z = vector3.z;
            return geometryVector3;
        }
    }
}
