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

        public float Shoulder_Pan_Offset_Position = (float)Math.PI;
        public float Shoulder_Lift_Offset_Position = (float)Math.PI / 2;
        public float Elbow_Offset_Position = 0.0f;
        public float Wrist_1_Offset_Position = (float)Math.PI / 2;
        public float Wrist_2_Offset_Position = 0.0f;
        public float Wrist_3_Offset_Position = -(float)Math.PI / 4;

        Dictionary<string, Transform> JointName_Dictionary = new Dictionary<string, Transform>();
        Dictionary<string, Vector3> JointAxis_Dictionary = new Dictionary<string, Vector3>();
        Dictionary<string, float> JointOffset_Dictionary = new Dictionary<string, float>();

        private void Start()
        {
            JointName_Dictionary.Add(prefix + "shoulder_pan_joint", Shoulder_Pan);
            JointName_Dictionary.Add(prefix + "shoulder_lift_joint", Shoulder_Lift);
            JointName_Dictionary.Add(prefix + "elbow_joint", Elbow);
            JointName_Dictionary.Add(prefix + "wrist_1_joint", Wrist_1);
            JointName_Dictionary.Add(prefix + "wrist_2_joint", Wrist_2);
            JointName_Dictionary.Add(prefix + "wrist_3_joint", Wrist_3);

            JointAxis_Dictionary.Add(prefix + "shoulder_pan_joint", Vector3.forward);
            JointAxis_Dictionary.Add(prefix + "shoulder_lift_joint", Vector3.up);
            JointAxis_Dictionary.Add(prefix + "elbow_joint", Vector3.up);
            JointAxis_Dictionary.Add(prefix + "wrist_1_joint", Vector3.up);
            JointAxis_Dictionary.Add(prefix + "wrist_2_joint", Vector3.forward);
            JointAxis_Dictionary.Add(prefix + "wrist_3_joint", Vector3.up);

            JointOffset_Dictionary.Add(prefix + "shoulder_pan_joint", Shoulder_Pan_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "shoulder_lift_joint", Shoulder_Lift_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "elbow_joint", Elbow_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "wrist_1_joint", Wrist_1_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "wrist_2_joint", Wrist_2_Offset_Position);
            JointOffset_Dictionary.Add(prefix + "wrist_3_joint", Wrist_3_Offset_Position);
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
                arm_transform.localEulerAngles = UpdateArmOrientation(JointAxis_Dictionary[_jointNames[k]], -1 * (float)_trajectory[0].joint_trajectory.points[_currentPointPosition].positions[k] + JointOffset_Dictionary[_jointNames[k]]);
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

