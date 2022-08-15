using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class DisplayTrajectoryPublisher : MonoBehaviour
    {
        private bool isMessageReceived;
        private MessageTypes.Moveit.RobotTrajectory[] _trajectory;
        private string[] _jointNames;
        private int _totalPoints;
        private double _totalTime;
        private float _stopWatch = 0;
        private int _currentPointPosition;

        public string prefix = "";
        public Transform Shoulder_Pan;
        public Transform Shoulder_Lift;
        public Transform Elbow;
        public Transform Wrist_1;
        public Transform Wrist_2;
        public Transform Wrist_3;
        public Transform Right_Bottom_Finger;
        public Transform Left_Bottom_Finger;

        private Vector3 InitialJoint_1;
        private Vector3 InitialJoint_2;
        private Vector3 InitialJoint_3;
        private Vector3 InitialJoint_4;
        private Vector3 InitialJoint_5;
        private Vector3 InitialJoint_6;
        private Vector3 Initial_Right_Finger_Bottom;
        private Vector3 Initial_Left_Finger_Bottom;

        public float Shoulder_Pan_Offset_Position = 0; //(float)Math.PI;
        public float Shoulder_Lift_Offset_Position = 0; // (float)Math.PI / 2;
        public float Elbow_Offset_Position = 0;
        public float Wrist_1_Offset_Position = 0; // (float)Math.PI / 2;
        public float Wrist_2_Offset_Position = 0;
        public float Wrist_3_Offset_Position = 0; //-(float)Math.PI / 4;
        public float Right_Bottom_Finger_Offset_Position = 0; //-(float)Math.PI / 4;
        public float Left_Bottom_Finger_Offset_Position = 0; //-(float)Math.PI / 4;


        Dictionary<string, Transform> JointName_Dictionary = new Dictionary<string, Transform>();
        Dictionary<string, Vector3> JointAxis_Dictionary = new Dictionary<string, Vector3>();
        Dictionary<string, float> JointOffset_Dictionary = new Dictionary<string, float>();
        Dictionary<string, Vector3> JointInitialRotation_Dictiontary = new Dictionary<string, Vector3>();


        private void Start()
        {
            InitialJoint_1 = Shoulder_Pan.localEulerAngles;
            InitialJoint_2 = Shoulder_Lift.localEulerAngles;
            InitialJoint_3 = Elbow.localEulerAngles;
            InitialJoint_4 = Wrist_1.localEulerAngles;
            InitialJoint_5 = Wrist_2.localEulerAngles;
            InitialJoint_6 = Wrist_3.localEulerAngles;
            Initial_Right_Finger_Bottom = Right_Bottom_Finger.localEulerAngles;
            Initial_Left_Finger_Bottom = Left_Bottom_Finger.localEulerAngles;

            JointName_Dictionary.Add(prefix + "joint_1", Shoulder_Pan);
            JointName_Dictionary.Add(prefix + "joint_2", Shoulder_Lift);
            JointName_Dictionary.Add(prefix + "joint_3", Elbow);
            JointName_Dictionary.Add(prefix + "joint_4", Wrist_1);
            JointName_Dictionary.Add(prefix + "joint_5", Wrist_2);
            JointName_Dictionary.Add(prefix + "joint_6", Wrist_3);
            JointName_Dictionary.Add(prefix + "left_finger_bottom_joint", Left_Bottom_Finger);
            JointName_Dictionary.Add(prefix + "right_finger_bottom_joint", Right_Bottom_Finger);


            JointAxis_Dictionary.Add(prefix + "joint_1", Vector3.up);
            JointAxis_Dictionary.Add(prefix + "joint_2", Vector3.right);
            JointAxis_Dictionary.Add(prefix + "joint_3", -1 * Vector3.up);
            JointAxis_Dictionary.Add(prefix + "joint_4", Vector3.right);
            JointAxis_Dictionary.Add(prefix + "joint_5", Vector3.forward);
            JointAxis_Dictionary.Add(prefix + "joint_6", -1 * Vector3.forward);
            JointAxis_Dictionary.Add(prefix + "left_finger_bottom_joint", Vector3.right);
            JointAxis_Dictionary.Add(prefix + "right_finger_bottom_joint", Vector3.right);


            JointOffset_Dictionary.Add(prefix + "joint_1", Shoulder_Pan_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "joint_2", Shoulder_Lift_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "joint_3", Elbow_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "joint_4", Wrist_1_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "joint_5", Wrist_2_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "joint_6", Wrist_3_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "left_finger_bottom_joint", Right_Bottom_Finger_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "right_finger_bottom_joint", Left_Bottom_Finger_Offset_Position);


            JointInitialRotation_Dictiontary.Add("joint_1", InitialJoint_1);
            JointInitialRotation_Dictiontary.Add("joint_2", InitialJoint_2);
            JointInitialRotation_Dictiontary.Add("joint_3", InitialJoint_3);
            JointInitialRotation_Dictiontary.Add("joint_4", InitialJoint_4);
            JointInitialRotation_Dictiontary.Add("joint_5", InitialJoint_5);
            JointInitialRotation_Dictiontary.Add("joint_6", InitialJoint_6);
            JointInitialRotation_Dictiontary.Add("left_finger_bottom_joint", Initial_Left_Finger_Bottom);
            JointInitialRotation_Dictiontary.Add("right_finger_bottom_joint", Initial_Right_Finger_Bottom);
        }

        private void Update()
        {
            if (isMessageReceived)
            {
                _stopWatch += Time.deltaTime;
                if(_stopWatch >= _totalTime)               
                    _stopWatch = 0;

                ProcessMessage();
            }
        }

        public void Write(MessageTypes.Moveit.DisplayTrajectory message)
        {
            _trajectory = message.trajectory;
            _jointNames = _trajectory[0].joint_trajectory.joint_names;
            _totalPoints = _trajectory[0].joint_trajectory.points.Length;
            _totalTime = _trajectory[0].joint_trajectory.points[_totalPoints - 1].time_from_start.secs + _trajectory[0].joint_trajectory.points[_totalPoints - 1].time_from_start.nsecs * 1e-9;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {    
            if (!JointName_Dictionary.ContainsKey(_jointNames[0]))
                return;
            
            _currentPointPosition = (int)Mathf.Lerp(0, _totalPoints, _stopWatch / (float)_totalTime);

            for (int k = 0; k < _jointNames.Length; k++)
            {
                var arm_transform = JointName_Dictionary[_jointNames[k]];
                arm_transform.localEulerAngles = JointInitialRotation_Dictiontary[_jointNames[k]] + UpdateArmOrientation(JointAxis_Dictionary[_jointNames[k]], -1 * (float)_trajectory[0].joint_trajectory.points[_currentPointPosition].positions[k] + JointOffset_Dictionary[_jointNames[k]]);
            }
        }

        private Vector3 UpdateArmOrientation(Vector3 axis, float position)
        {
            if (position < 0)
                return axis * (position + 2 * (float)Math.PI) * (180.0f / (float)Math.PI);
            else
                return axis * position * (180.0f / (float)Math.PI);
        }
    }
}

