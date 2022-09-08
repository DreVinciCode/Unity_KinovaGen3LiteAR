using UnityEngine;
using TMPro;

namespace RosSharp.RosBridgeClient
{
    public class EyeGazeTeleopPublisher : UnityPublisher<MessageTypes.Geometry.Twist>
    {
        public GameObject EndEffectorPoint;
        public GameObject EyeTarget_Cursor;
        public TMP_Text Status;
        public TMP_Text Direction;

        public bool _publishMessageCheck { get; set; }
        public bool _objectDetected { get; set; }
        public bool _activeDwell { get; set; }
        public bool _activeState { get; set; }
        public bool x_fixed { get; set; }
        public bool y_fixed { get; set; }
        public bool z_fixed { get; set; }

        private MessageTypes.Geometry.Twist message;
        private float _linearX = 0;
        private float _linearY = 0;
        private float _linearZ = 0;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Twist();
        }

        private void PublishMessage()
        {
            if (!x_fixed)
                _linearX = 0;

            if (!y_fixed)
                _linearY = 0;

            if (!z_fixed)
                _linearZ = 0;

            Direction.text = "x : " + (Mathf.Round((_linearX) * 100f) / 100f).ToString() + " \ny: " + (Mathf.Round((_linearY) * 100f) / 100f).ToString() + "\nz: " + (Mathf.Round((_linearZ) * 100f) / 100f).ToString();

            Vector3 linearVelocity = new Vector3(_linearX, _linearY, _linearZ);
            Vector3 angularVelocity = new Vector3(0f, 0f, 0f);

            message.linear = GetGeometryVector3(linearVelocity);
            message.angular = GetGeometryVector3(angularVelocity);

            if (_publishMessageCheck)
                Publish(message);
        }

        private void Update()
        {
            if (!_activeState || !_objectDetected)
            {
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                PublishMessage();
            }
            
            if (_activeState)
            {
                Status.text = "Active";
            }
            else
            {
                Status.text = "Inactive";
            }


            if (_activeState && _activeDwell)
            {
                var z_arm_difference = EyeTarget_Cursor.transform.position.y - EndEffectorPoint.transform.position.y;
                var y_arm_difference = EyeTarget_Cursor.transform.position.x - EndEffectorPoint.transform.position.x;
                var x_arm_difference = -(EyeTarget_Cursor.transform.position.z - EndEffectorPoint.transform.position.z);

                _linearZ = z_arm_difference;
                _linearX = x_arm_difference;
                _linearY = y_arm_difference;


                PublishMessage();
            }
            else
            {
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
                PublishMessage();
                return;
            }
        }

        private static MessageTypes.Geometry.Vector3 GetGeometryVector3(Vector3 vector3)
        {
            MessageTypes.Geometry.Vector3 geometryVector3 = new MessageTypes.Geometry.Vector3();
            geometryVector3.x = vector3.x;
            geometryVector3.y = vector3.y;
            geometryVector3.z = vector3.z;
            return geometryVector3;
        }

        public void Test()
        {
            Debug.Log("TEst");
        }
    }
}
