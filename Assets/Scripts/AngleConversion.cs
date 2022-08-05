using System;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class AngleConversion : MonoBehaviour
    {
        public Transform Left_Shoulder_Pan;
        public Transform Left_Shoulder_Lift;
        public Transform Left_Elbow;
        public Transform Left_Wrist_1;
        public Transform Left_Wrist_2;
        public Transform Left_Wrist_3;



        [Range(-6.28f, 6.28f)] public float Left_Shoulder_Pan_position;
        [Range(-6.28f, 6.28f)] public float Left_Shoulder_Lift_position;
        [Range(-3.14f, 3.14f)] public float Left_Elbow_position;
        [Range(-6.28f, 6.28f)] public float Left_Wrist_1_position;
        [Range(-6.28f, 6.28f)] public float Left_Wrist_2_position;
        [Range(-6.28f, 6.28f)] public float Left_Wrist_3_position;


        //Setting Offset value for joint values 
        private float _right_Shoulder_Pan_Offset_Position = (float)Math.PI;
        private float _right_Shoulder_Lift_Offset_Position = (float)Math.PI/2;
        private float _right_Elbow_Offset_Position = 0.0f;
        private float _right_Wrist_1_Offset_Position = (float)Math.PI/2;
        private float _right_Wrist_2_Offset_Position = 0.0f;
        private float _right_Wrist_3_Offset_Position = -(float)Math.PI/4;


        private Vector3 InitialJoint_1;
        private Vector3 InitialJoint_2;
        private Vector3 InitialJoint_3;
        private Vector3 InitialJoint_4;
        private Vector3 InitialJoint_5;
        private Vector3 InitialJoint_6;



        private void Start()
        {
            InitialJoint_1 = Left_Shoulder_Pan.localEulerAngles;
            InitialJoint_2 = Left_Shoulder_Lift.localEulerAngles;
            InitialJoint_3 = Left_Elbow.localEulerAngles;
            InitialJoint_4 = Left_Wrist_1.localEulerAngles;
            InitialJoint_5 = Left_Wrist_2.localEulerAngles;
            InitialJoint_6 = Left_Wrist_3.localEulerAngles;
        }

        private void Update()
        {
            Left_Shoulder_Pan.localEulerAngles = InitialJoint_1 + UpdateArmOrientation(-1 * Vector3.up, Left_Shoulder_Pan_position);
            Left_Shoulder_Lift.localEulerAngles = InitialJoint_2 + UpdateArmOrientation(Vector3.right, Left_Shoulder_Lift_position);
            Left_Elbow.localEulerAngles = InitialJoint_3 + UpdateArmOrientation(-1 * Vector3.up, Left_Elbow_position);
            Left_Wrist_1.localEulerAngles = InitialJoint_4 + UpdateArmOrientation(-1 * Vector3.right, Left_Wrist_1_position);
            Left_Wrist_2.localEulerAngles = InitialJoint_5 +  UpdateArmOrientation(Vector3.forward, Left_Wrist_2_position);
            Left_Wrist_3.localEulerAngles = InitialJoint_6 +  UpdateArmOrientation(Vector3.forward, Left_Wrist_3_position);
        }

        private float RadianToDegree(float position)
        {
            var radian = -1 * position;
            if (radian < 0)
                return (radian + 2 * (float)Math.PI) * (180.0f / (float)Math.PI);
            else
                return radian * (180.0f / (float)Math.PI);
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
