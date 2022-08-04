using System;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointTrajectoryControllerStatePublisher : MonoBehaviour
    {
        private bool isMessageReceived;
        private int _jointLength;
        private double[] _positions;
        private string[] _jointNames;

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
            JointAxis_Dictionary.Add(prefix + "wrist_1_joint",  Vector3.up);
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
                ProcessMessage();
        }

        public void Write(MessageTypes.Control.JointTrajectoryControllerState message)
        {
            _jointNames = message.joint_names;
            _positions =  message.desired.positions;
            _jointLength = _positions.Length;

            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            for (int i = 0; i < _jointLength; i++)
            {
                var arm_transform = JointName_Dictionary[_jointNames[i]];
                arm_transform.localEulerAngles = UpdateArmOrientation(JointAxis_Dictionary[_jointNames[i]], -1 * (float)_positions[i] + JointOffset_Dictionary[_jointNames[i]]);
            }

            isMessageReceived = false;
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
