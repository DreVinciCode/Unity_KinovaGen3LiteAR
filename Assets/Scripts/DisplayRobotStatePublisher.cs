using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class DisplayRobotStatePublisher : MonoBehaviour
    {
        private bool isMessageReceived;
        private string[] _joint_names;

        private MessageTypes.Moveit.RobotState _state;

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }


        public void Write(MessageTypes.Moveit.DisplayRobotState message)
        {
            _state = message.state;
            _joint_names = _state.joint_state.name;

            isMessageReceived = true;
        }


        private void ProcessMessage()
        {
            Debug.Log("Joint names: " + _joint_names);
            isMessageReceived = false;

        }
    }
}

