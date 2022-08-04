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


        private void Update()
        {
  
            Left_Shoulder_Pan.localEulerAngles = UpdateArmOrientation(-1 * Vector3.up, Left_Shoulder_Pan_position);
            Left_Shoulder_Lift.localEulerAngles = UpdateArmOrientation(Vector3.forward, Left_Shoulder_Lift_position);
            Left_Elbow.localEulerAngles = UpdateArmOrientation(-1 * Vector3.up, Left_Elbow_position);
            Left_Wrist_1.localEulerAngles = UpdateArmOrientation(-1 * Vector3.up, Left_Wrist_1_position);
            Left_Wrist_2.localEulerAngles = UpdateArmOrientation(-1 * Vector3.forward, Left_Wrist_2_position);
            Left_Wrist_3.localEulerAngles = UpdateArmOrientation(Vector3.up, Left_Wrist_3_position);
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
