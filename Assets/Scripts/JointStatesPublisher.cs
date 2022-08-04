using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointStatesPublisher : MonoBehaviour
    {
        private bool isMessageReceived;
        private string[] _jointNames;
        private double[] _jointPositions;

        public Transform Right_Shoulder_Pan;
        public Transform Right_Shoulder_Lift;
        public Transform Right_Elbow;
        public Transform Right_Wrist_1;
        public Transform Right_Wrist_2;
        public Transform Right_Wrist_3;

        public float Right_Shoulder_Pan_Offset_Position = (float)Math.PI;
        public float Right_Shoulder_Lift_Offset_Position = (float)Math.PI / 2;
        public float Right_Elbow_Offset_Position = 0.0f;
        public float Right_Wrist_1_Offset_Position = (float)Math.PI / 2;
        public float Right_Wrist_2_Offset_Position = 0.0f;
        public float Right_Wrist_3_Offset_Position = -(float)Math.PI / 4;

        public Transform Left_Shoulder_Pan;
        public Transform Left_Shoulder_Lift;
        public Transform Left_Elbow;
        public Transform Left_Wrist_1;
        public Transform Left_Wrist_2;
        public Transform Left_Wrist_3;

        public float Left_Shoulder_Pan_Offset_Position = (float)Math.PI;
        public float Left_Shoulder_Lift_Offset_Position = (float)Math.PI / 2;
        public float Left_Elbow_Offset_Position = 0.0f;
        public float Left_Wrist_1_Offset_Position = (float)Math.PI / 2;
        public float Left_Wrist_2_Offset_Position = 0.0f;
        public float Left_Wrist_3_Offset_Position = -(float)Math.PI / 4;

        Dictionary<string, Transform> JointName_Dictionary = new Dictionary<string, Transform>();
        Dictionary<string, Vector3> JointAxis_Dictionary = new Dictionary<string, Vector3>();
        Dictionary<string, float> JointOffset_Dictionary = new Dictionary<string, float>();

        private void Start()
        {
            
            JointName_Dictionary.Add("right_shoulder_pan_joint", Right_Shoulder_Pan);
            JointName_Dictionary.Add("right_shoulder_lift_joint", Right_Shoulder_Lift);
            JointName_Dictionary.Add("right_elbow_joint", Right_Elbow);
            JointName_Dictionary.Add("right_wrist_1_joint", Right_Wrist_1);
            JointName_Dictionary.Add("right_wrist_2_joint", Right_Wrist_2);
            JointName_Dictionary.Add("right_wrist_3_joint", Right_Wrist_3);

            JointAxis_Dictionary.Add("right_shoulder_pan_joint", Vector3.forward);
            JointAxis_Dictionary.Add("right_shoulder_lift_joint", Vector3.up);
            JointAxis_Dictionary.Add("right_elbow_joint", Vector3.up);
            JointAxis_Dictionary.Add("right_wrist_1_joint", Vector3.up);
            JointAxis_Dictionary.Add("right_wrist_2_joint", Vector3.forward);
            JointAxis_Dictionary.Add("right_wrist_3_joint", Vector3.up);

            JointOffset_Dictionary.Add("right_shoulder_pan_joint", Right_Shoulder_Pan_Offset_Position);
            JointOffset_Dictionary.Add("right_shoulder_lift_joint", Right_Shoulder_Lift_Offset_Position);
            JointOffset_Dictionary.Add("right_elbow_joint", Right_Elbow_Offset_Position);
            JointOffset_Dictionary.Add("right_wrist_1_joint", Right_Wrist_1_Offset_Position);
            JointOffset_Dictionary.Add("right_wrist_2_joint", Right_Wrist_2_Offset_Position);
            JointOffset_Dictionary.Add("right_wrist_3_joint", Right_Wrist_3_Offset_Position);

            JointName_Dictionary.Add("left_shoulder_pan_joint", Left_Shoulder_Pan);
            JointName_Dictionary.Add("left_shoulder_lift_joint", Left_Shoulder_Lift);
            JointName_Dictionary.Add("left_elbow_joint", Left_Elbow);
            JointName_Dictionary.Add("left_wrist_1_joint", Left_Wrist_1);
            JointName_Dictionary.Add("left_wrist_2_joint", Left_Wrist_2);
            JointName_Dictionary.Add("left_wrist_3_joint", Left_Wrist_3);

            JointAxis_Dictionary.Add("left_shoulder_pan_joint", Vector3.forward);
            JointAxis_Dictionary.Add("left_shoulder_lift_joint", Vector3.up);
            JointAxis_Dictionary.Add("left_elbow_joint", Vector3.up);
            JointAxis_Dictionary.Add("left_wrist_1_joint", Vector3.up);
            JointAxis_Dictionary.Add("left_wrist_2_joint", Vector3.forward);
            JointAxis_Dictionary.Add("left_wrist_3_joint", Vector3.up);

            JointOffset_Dictionary.Add("left_shoulder_pan_joint", Left_Shoulder_Pan_Offset_Position);
            JointOffset_Dictionary.Add("left_shoulder_lift_joint", Left_Shoulder_Lift_Offset_Position);
            JointOffset_Dictionary.Add("left_elbow_joint", Left_Elbow_Offset_Position);
            JointOffset_Dictionary.Add("left_wrist_1_joint", Left_Wrist_1_Offset_Position);
            JointOffset_Dictionary.Add("left_wrist_2_joint", Left_Wrist_2_Offset_Position);
            JointOffset_Dictionary.Add("left_wrist_3_joint", Left_Wrist_3_Offset_Position);
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        public void Write(MessageTypes.Sensor.JointState message)
        {
            _jointNames = message.name;
            _jointPositions = message.position;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            for (int i = 0; i < _jointNames.Length; i++)
            {
                if (JointName_Dictionary.ContainsKey(_jointNames[i]))
                {
                    var arm_transform = JointName_Dictionary[_jointNames[i]];
                    arm_transform.localEulerAngles = UpdateArmOrientation(JointAxis_Dictionary[_jointNames[i]], -1 * (float)_jointPositions[i] + JointOffset_Dictionary[_jointNames[i]]);
                }
            }
        }

        private Vector3 UpdateArmOrientation(Vector3 axis, float position)
        {
            //positions are in radians; convert to degrees
            if (position < 0)
                return axis * (position + 2 * (float)Math.PI) * (180.0f / (float)Math.PI);
            else
                return axis * position * (180.0f / (float)Math.PI);
        }
    }
}

