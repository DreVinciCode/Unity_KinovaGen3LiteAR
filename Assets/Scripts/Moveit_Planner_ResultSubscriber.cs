using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class Moveit_Planner_ResultSubscriber : UnitySubscriber<MessageTypes.Std.Int16>
    {
        public Moveit_Planner_ResultPublisher moveit_planner_result;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.Int16 message)
        {
            moveit_planner_result.Write(message);
        }
    }
}