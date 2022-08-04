using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class JointStatePublisher_Kinova_gen3Lite : MonoBehaviour
    {

        private bool isMessageReceived;
        private string[] _jointNames;
        private double[] _jointPositions;

        public Transform Joint_1;
        public Transform Joint_2;
        public Transform Joint_3;
        public Transform Joint_4;
        public Transform Joint_5;
        public Transform Joint_6;
        
        /*
        public Transform right_finger_buttom_joint;
        public Transform right_finger_tip_joint;
        public Transform left_finger_buttom_joint;
        public Transform left_finger_tip_joint;
        */

        public float Joint_1_Offset_Position;
        public float Joint_2_Offset_Position = (float)Math.PI / 2;
        public float Joint_3_Offset_Position; 
        public float Joint_4_Offset_Position;
        public float Joint_5_Offset_Position; 
        public float Joint_6_Offset_Position;

        Dictionary<string, Transform> JointName_Dictionary = new Dictionary<string, Transform>();
        Dictionary<string, Vector3> JointAxis_Dictionary = new Dictionary<string, Vector3>();
        Dictionary<string, float> JointOffset_Dictionary = new Dictionary<string, float>();

        private void Start()
        {
            JointName_Dictionary.Add("joint_1", Joint_1);
            JointName_Dictionary.Add("joint_2", Joint_2);
            JointName_Dictionary.Add("joint_3", Joint_3);
            JointName_Dictionary.Add("joint_4", Joint_4);
            JointName_Dictionary.Add("joint_5", Joint_5);
            JointName_Dictionary.Add("joint_6", Joint_6);

            JointAxis_Dictionary.Add("joint_1", Vector3.up);
            JointAxis_Dictionary.Add("joint_2", Vector3.right);
            JointAxis_Dictionary.Add("joint_3", Vector3.forward);
            JointAxis_Dictionary.Add("joint_4", Vector3.forward);
            JointAxis_Dictionary.Add("joint_5", Vector3.forward);
            JointAxis_Dictionary.Add("joint_6", Vector3.forward);

            JointOffset_Dictionary.Add("joint_1", Joint_1_Offset_Position);
            JointOffset_Dictionary.Add("joint_2", Joint_2_Offset_Position);
            JointOffset_Dictionary.Add("joint_3", Joint_3_Offset_Position);
            JointOffset_Dictionary.Add("joint_4", Joint_4_Offset_Position);
            JointOffset_Dictionary.Add("joint_5", Joint_5_Offset_Position);
            JointOffset_Dictionary.Add("joint_6", Joint_6_Offset_Position);
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
            for (int i = 0; i < 2; i++)
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
