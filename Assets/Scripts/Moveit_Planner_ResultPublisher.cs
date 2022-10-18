using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class Moveit_Planner_ResultPublisher : MonoBehaviour
    {
        public ChangeMaterialColor changeMaterialColor;

        public void Write(MessageTypes.Std.Int16 message)
        {
            int value = (int)message.data;
            if (value == -1)
                changeMaterialColor.ResultPlannerColor();
        }
    }
}
