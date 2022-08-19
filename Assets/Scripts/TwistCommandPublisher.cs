using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace RosSharp.RosBridgeClient
{
    public class TwistCommandPublisher : UnityPublisher<MessageTypes.Geometry.Twist>
    {

        public GameObject LeftHandThumbMarker;
        public GameObject LeftHandWristMarker;
        public GameObject LeftHandIndexMarker;
        public TMP_Text Status;
        public TMP_Text Direction;

        public bool _publishMessageCheck { get; set; }

        private MessageTypes.Geometry.Twist message;


        private uint _referenceFrame = 0;
        private uint _duration = 0;
        private bool _activeState = false;
        private float _rotationFactor = 1f;
        private float _timeCounter1 = 0;
        private float _timeCounter2 = 0;
        private float left_thumbCurl;
        private float left_indexCurl;
        private float left_middleCurl;
        private float left_ringCurl;
        private float left_pinkyCurl;
        private float _linearX = 0;
        private float _linearY = 0;
        private float _linearZ = 0;
        private float _maxVelocity = 0.02f;

        private MixedRealityPose _leftThumbPose, _leftWristPose, _leftIndexPose, _leftMiddlePose;

        public UnityEvent _StopEvent;
        public UnityEvent _EmergencyStopEvent;

        GameObject _leftThumbObject;
        GameObject _leftWristObject;
        GameObject _leftIndexObject;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            HandGestureSetup();

            _StopEvent = new UnityEvent();
            _EmergencyStopEvent = new UnityEvent();
        }

        private void HandGestureSetup()
        {
            _leftThumbObject = Instantiate(LeftHandThumbMarker, Camera.main.transform);
            _leftWristObject = Instantiate(LeftHandWristMarker, Camera.main.transform);
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
            message.angular = GetGeometryVector3(-angularVelocity);

            Publish(message);
        }

        private bool ActivateTeleop()
        {
            if (!(left_indexCurl < 0.1f && left_middleCurl < 0.1f && left_ringCurl < 0.1f && left_pinkyCurl < 0.1f && left_thumbCurl < 0.4f))
            {
                _timeCounter1 = 0;
                return false;
            }

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Left, out _leftThumbPose) &&
                   HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Left, out _leftWristPose))
            {
                _leftThumbObject.transform.position = _leftThumbPose.Position;
                _leftWristObject.transform.position = _leftWristPose.Position;
            }
            else
            {
                _timeCounter1 = 0;
                return false;
            }

            if (_leftThumbObject.transform.localPosition.x < _leftWristObject.transform.localPosition.x)
            {
                _timeCounter1 += Time.deltaTime;
            }
            else
            {
                _timeCounter1 = 0;
                return false;
            }

            if (_timeCounter1 >= 3f)
            {
                _activeState = true;
                _timeCounter1 = 0;
                return true;
            }
            else { return false; }

        }

        private void DeactivateTeleop()
        {
            if (!(left_indexCurl < 0.1f && left_middleCurl < 0.1f && left_ringCurl < 0.1f && left_pinkyCurl < 0.1f && left_thumbCurl < 0.4f))
            {
                _timeCounter2 = 0;
                return;
            }

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Left, out _leftThumbPose) &&
                   HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Left, out _leftWristPose))
            {
                _leftThumbObject.transform.position = _leftThumbPose.Position;
                _leftWristObject.transform.position = _leftWristPose.Position;
            }
            else
            {
                _timeCounter2 = 0;
                return;
            }

            if (_leftThumbObject.transform.localPosition.x > _leftWristObject.transform.localPosition.x)
            {
                _timeCounter2 += Time.deltaTime;
            }
            else
            {
                _timeCounter2 = 0;
                return;
            }

            if (_timeCounter2 >= 3f)
            {
                _activeState = false;
                _timeCounter2 = 0;
            }
        }
   
        private void CartisianVelocityController()
        {
            if (left_thumbCurl == 0)
            {
                return;
            }

            //Index Pointing 
            if (left_indexCurl < 0.2f && left_middleCurl > 0.5f)
            {
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out _leftIndexPose) &&
                    HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Left, out _leftWristPose))
                {
                    _leftIndexObject.transform.position = _leftIndexPose.Position;
                    _leftWristObject.transform.position = _leftWristPose.Position;
                    _leftIndexObject.GetComponent<Renderer>().enabled = true;


                    var y_distance = Mathf.Abs(_leftIndexObject.transform.localPosition.y - _leftWristObject.transform.localPosition.y);
                    var z_distance = Mathf.Abs(_leftIndexObject.transform.localPosition.z - _leftWristObject.transform.localPosition.z);


                    //Implied forward
                    if(z_distance > y_distance)
                    {
                        if(_leftIndexObject.transform.localPosition.z > _leftWristObject.transform.localPosition.z)
                        {
                            _linearX = _maxVelocity;
                            _linearY = 0;
                            _linearZ = 0;
                            Direction.text = "Forward: \n" + _linearX.ToString();
                        }
                        if(_leftIndexObject.transform.localPosition.z < _leftWristObject.transform.localPosition.z)
                        {
                            _linearX = -1 * _maxVelocity;
                            _linearY = 0;
                            _linearZ = 0;
                            Direction.text = "Backwards: \n" + _linearX.ToString();
                        }                      
                    }

                    //Implied Veritical Direction
                    else if(y_distance > z_distance)
                    {
                        if (_leftIndexObject.transform.localPosition.y > _leftWristObject.transform.localPosition.y)
                        {
                            _linearX = 0;
                            _linearY = 0;
                            _linearZ = _maxVelocity;
                            Direction.text = "Up: \n" + _linearZ.ToString();
                        }
                        if (_leftIndexObject.transform.localPosition.z < _leftWristObject.transform.localPosition.z)
                        {
                            _linearX = 0;
                            _linearY = 0;
                            _linearZ = -1 * _maxVelocity;
                            Direction.text = "Down: \n" + _linearZ.ToString();
                        }
                    }

                    /*
                    //Verical direction
                    if (_leftIndexObject.transform.localPosition.z < _leftWristObject.transform.localPosition.z)
                    {
                        _linearX = 0;
                        _linearY = 0;
                        _linearZ = 0;
                        Direction.text = "Forward: \n" + _linearX.ToString();
                        return;
                    }

                    //Forward Direction (x-direction)
                    var calculated_Speed = (0.8f * Vector3.Distance(_leftIndexObject.transform.position, Camera.main.transform.position)) - 0.1f;

                    if (calculated_Speed > 0)
                        _linearX = _maxVelocity;
                    else
                        _linearX = 0;
                    */


                    /*
                    //Determine Direction
                    _linearY = (1) * (_rotationFactor) * Mathf.Round((_leftIndexObject.transform.localPosition.x - _leftWristObject.transform.localPosition.x) * 100f) / 100f;                  
                    if (_linearY > 0)
                    {
                        _linearY = _maxVelocity;
                        Direction.text = "Forward: \n" + _linearX.ToString() + "\nLeft: \n" + _linearY.ToString();
                    }
                    else if (_linearY < 0)
                    {
                        _linearY = (-1) * _maxVelocity;
                        Direction.text = "Forward: \n" + _linearX.ToString() + "\nRight: \n" + _linearY.ToString();
                    }
                    */
                }
            }
            else
            {
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                Direction.text = "Forward: \n" + _linearX.ToString();
            }

            //Flat palm to stop
            if (left_indexCurl < 0.1f && left_middleCurl < 0.1f && left_ringCurl < 0.1f && left_pinkyCurl < 0.1f && left_thumbCurl < 0.4f)
            {
                //Call the stop command?
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                Direction.text = "Stop";
            }

            //Thumb Pointing
            if (left_indexCurl > 0.7f && left_middleCurl > 0.7f && left_ringCurl > 0.7f && left_pinkyCurl > 0.7f)
            {
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Left, out _leftThumbPose) &&
                    HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Left, out _leftWristPose))
                {
                    _leftThumbObject.transform.position = _leftThumbPose.Position;
                    _leftWristObject.transform.position = _leftWristPose.Position;

                    var rotation_z = _leftWristPose.Rotation.eulerAngles.z;

                    //Right Turn
                    if ((_leftThumbObject.transform.localPosition.x > _leftWristObject.transform.localPosition.x) && (_leftThumbObject.transform.localPosition.z > _leftWristObject.transform.localPosition.z))
                    {
                        if ((rotation_z > 0 && rotation_z < 95) || (rotation_z < 360 && rotation_z > 350))
                        {
                            _linearX = 0;
                            _linearY = -1 * _maxVelocity;
                            _linearZ = 0;
                            //_angularValue = -1 * (-0.005f) * rotation_z + 0.5f;
                            Direction.text = "Right\n" + _linearY.ToString();
                        }
                    }
                    //Left Turn
                    else if ((_leftThumbObject.transform.localPosition.x < _leftWristObject.transform.localPosition.x) && (_leftThumbObject.transform.localPosition.z > _leftWristObject.transform.localPosition.z))
                    {
                        if (rotation_z < 190 && rotation_z > 105)
                        {
                            _linearX = 0;
                            _linearY = _maxVelocity;
                            _linearZ = 0;
                            //_angularValue = -1 * ((0.00625f) * rotation_z + (-0.625f));
                            Direction.text = "Left\n" + _linearY.ToString();
                        }
                    }
                    //Backwards
                    else if (_leftThumbObject.transform.localPosition.z < _leftWristObject.transform.localPosition.z)
                    {
                        var thumbWrist_Distance = _leftThumbObject.transform.localPosition.z - _leftWristObject.transform.localPosition.z;

                        //_linearValue = (7.14f) * thumbWrist_Distance;
                        //_linearValue = Mathf.Round((7.14f) * thumbWrist_Distance * 100f) / 100f;
                        _linearX = -1 * _maxVelocity;
                        Direction.text = "Backwards\n" + _linearX.ToString();

                        /*
                        //Determine Direction
                        _angularValue = (-1) * (_rotationFactor) * Mathf.Round((_leftThumbObject.transform.localPosition.x - _leftWristObject.transform.localPosition.x) * 100f) / 100f;

                        if (_angularValue < 0)
                        {
                            Direction.text = "Backwards: \n" + _linearValue.ToString() + "\nRight: \n" + _angularValue.ToString();
                        }
                        else if (_angularValue > 0)
                        {
                            Direction.text = "Backwards: \n" + _linearValue.ToString() + "\nLeft: \n" + _angularValue.ToString();
                        }
                        */
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            left_thumbCurl = HandPoseUtils.ThumbFingerCurl(Handedness.Left);
            left_indexCurl = HandPoseUtils.IndexFingerCurl(Handedness.Left);
            left_middleCurl = HandPoseUtils.MiddleFingerCurl(Handedness.Left);
            left_ringCurl = HandPoseUtils.RingFingerCurl(Handedness.Left);
            left_pinkyCurl = HandPoseUtils.PinkyFingerCurl(Handedness.Left);

            _leftThumbObject.GetComponent<Renderer>().enabled = false;
            _leftWristObject.GetComponent<Renderer>().enabled = false;
            _leftIndexObject.GetComponent<Renderer>().enabled = false;

            
            if (_activeState)
                Status.text = "Active";
            else
                Status.text = "Inactive";

            //Check for hand gesture to set active status; 
            if (!ActivateTeleop() && !_activeState)
            {
                return;
            }

            if (_activeState)
            {
                DeactivateTeleop();
                CartisianVelocityController();
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
