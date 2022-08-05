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

        private Vector3 InitialJoint_1;
        private Vector3 InitialJoint_2;
        private Vector3 InitialJoint_3;
        private Vector3 InitialJoint_4;
        private Vector3 InitialJoint_5;
        private Vector3 InitialJoint_6;

        /*
        public Transform right_finger_buttom_joint;
        public Transform right_finger_tip_joint;
        public Transform left_finger_buttom_joint;
        public Transform left_finger_tip_joint;
        */

        public float Joint_1_Offset_Position;
        public float Joint_2_Offset_Position = (float)Math.PI;
        public float Joint_3_Offset_Position; 
        public float Joint_4_Offset_Position;
        public float Joint_5_Offset_Position = (float)Math.PI; 
        public float Joint_6_Offset_Position;

        Dictionary<string, Transform> JointName_Dictionary = new Dictionary<string, Transform>();
        Dictionary<string, Vector3> JointAxis_Dictionary = new Dictionary<string, Vector3>();
        Dictionary<string, float> JointOffset_Dictionary = new Dictionary<string, float>();
        Dictionary<string, Vector3> JointInitialRotation_Dictiontary = new Dictionary<string, Vector3>();

        private void Start()
        {
            InitialJoint_1 = Joint_1.localEulerAngles;
            InitialJoint_2 = Joint_2.localEulerAngles;
            InitialJoint_3 = Joint_3.localEulerAngles;
            InitialJoint_4 = Joint_4.localEulerAngles;
            InitialJoint_5 = Joint_5.localEulerAngles;
            InitialJoint_6 = Joint_6.localEulerAngles;

            JointName_Dictionary.Add("joint_1", Joint_1);
            JointName_Dictionary.Add("joint_2", Joint_2);
            JointName_Dictionary.Add("joint_3", Joint_3);
            JointName_Dictionary.Add("joint_4", Joint_4);
            JointName_Dictionary.Add("joint_5", Joint_5);
            JointName_Dictionary.Add("joint_6", Joint_6);

            JointAxis_Dictionary.Add("joint_1", Vector3.up);
            JointAxis_Dictionary.Add("joint_2", Vector3.right);
            JointAxis_Dictionary.Add("joint_3", -1* Vector3.up);
            JointAxis_Dictionary.Add("joint_4", Vector3.right);
            JointAxis_Dictionary.Add("joint_5", Vector3.forward);
            JointAxis_Dictionary.Add("joint_6", -1*  Vector3.forward);

            JointOffset_Dictionary.Add("joint_1", Joint_1_Offset_Position);
            JointOffset_Dictionary.Add("joint_2", Joint_2_Offset_Position);
            JointOffset_Dictionary.Add("joint_3", Joint_3_Offset_Position);
            JointOffset_Dictionary.Add("joint_4", Joint_4_Offset_Position);
            JointOffset_Dictionary.Add("joint_5", Joint_5_Offset_Position);
            JointOffset_Dictionary.Add("joint_6", Joint_6_Offset_Position);

            JointInitialRotation_Dictiontary.Add("joint_1", InitialJoint_1);
            JointInitialRotation_Dictiontary.Add("joint_2", InitialJoint_2);
            JointInitialRotation_Dictiontary.Add("joint_3", InitialJoint_3);
            JointInitialRotation_Dictiontary.Add("joint_4", InitialJoint_4);
            JointInitialRotation_Dictiontary.Add("joint_5", InitialJoint_5);
            JointInitialRotation_Dictiontary.Add("joint_6", InitialJoint_6);

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
                    arm_transform.localEulerAngles =  JointInitialRotation_Dictiontary[_jointNames[i]] +  UpdateArmOrientation(JointAxis_Dictionary[_jointNames[i]], -1 * (float)_jointPositions[i] + JointOffset_Dictionary[_jointNames[i]]);
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
