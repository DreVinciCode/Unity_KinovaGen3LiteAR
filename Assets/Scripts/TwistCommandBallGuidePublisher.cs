using UnityEngine;
using TMPro;


namespace RosSharp.RosBridgeClient
{
    public class TwistCommandBallGuidePublisher : UnityPublisher<MessageTypes.Geometry.Twist>
    {
        public GameObject SphereStartingPosition;
        public GameObject TargetSphere;

        public TMP_Text Mode;
        public TMP_Text Direction;

        private Vector3 InitialPosition;
        private Vector3 DirectionVector;

        private bool _activeState;

        private MessageTypes.Geometry.Twist message;
        private float _linearX = 0;
        private float _linearY = 0;
        private float _linearZ = 0;

        private float _minRange = 0.02f;
        private float _maxVelocity = 0.02f;
        private float _velocityFactor = 1f;

        public bool _publishMessageCheck { get; set; }


        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            _activeState = false;
            InitialPosition = TargetSphere.transform.localPosition;
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Twist();
        }

        private void PublishMessage()
        {
            Vector3 linearVelocity = new Vector3(_linearX, _linearY, _linearZ);
            Vector3 angularVelocity = new Vector3(0f, 0f, 0f);

            message.linear = GetGeometryVector3(linearVelocity);
            message.angular = GetGeometryVector3(angularVelocity);

            Publish(message);
        }

        private void Update()
        {            
            if(!_activeState)
            {
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
            }

            DirectionVector = (TargetSphere.transform.localPosition - InitialPosition);

            var x_vector = DirectionVector.z;
            var y_vector = DirectionVector.x;
            var z_vector = DirectionVector.y;

            if (Vector3.Magnitude(DirectionVector) > _minRange)
            {
                // X axis
                if (Mathf.Abs(x_vector) > Mathf.Abs(y_vector) && Mathf.Abs(x_vector) > Mathf.Abs(z_vector))
                {
                    _linearX = x_vector;
                    _linearY = 0;
                    _linearZ = 0;

                    if (DirectionVector.x > 0)                   
                        Direction.text = "Forward";
                    else            
                        Direction.text = "Backwards";
                }
                // Y axis
                else if (Mathf.Abs(y_vector) > Mathf.Abs(x_vector) && Mathf.Abs(y_vector) > Mathf.Abs(z_vector))
                {
                    _linearX = 0;
                    _linearY = y_vector;
                    _linearZ = 0;

                    if (DirectionVector.y > 0)
                        Direction.text = "Left";
                    else
                        Direction.text = "Right";
                }
                // Z axis
                else if (Mathf.Abs(z_vector) > Mathf.Abs(y_vector) && Mathf.Abs(z_vector) > Mathf.Abs(x_vector))
                {
                    _linearX = 0;
                    _linearY = 0;
                    _linearZ = z_vector;

                    if (DirectionVector.x > 0)
                        Direction.text = "Up";
                    else                
                        Direction.text = "Down";                    
                }
            }
            else
            {
                Direction.text = "--";
                _linearX = 0;
                _linearY = 0;
                _linearZ = 0;
            }
            
            if (_publishMessageCheck)
                PublishMessage();
        }

        public void ResetPosition()
        {
            //TargetSphere.transform.localPosition = SphereStartingPosition.transform.localPosition;
            TargetSphere.transform.position = Camera.main.transform.position + new Vector3(0, -0.1f, 0.4f);
            Direction.text = "--";
            _linearX = 0;
            _linearY = 0;
            _linearZ = 0;
        }

        public void SetInitialPosition()
        {
            InitialPosition = TargetSphere.transform.localPosition;
        }

        public void ToggleActive()
        {
            _activeState = true;
        }

        public void ToggleDeactive()
        {
            _activeState = false;
            DirectionVector = Vector3.zero;
        }

        private static MessageTypes.Geometry.Vector3 GetGeometryVector3(Vector3 vector3)
        {
            MessageTypes.Geometry.Vector3 geometryVector3 = new MessageTypes.Geometry.Vector3();
            geometryVector3.x = vector3.x;
            geometryVector3.y = vector3.y;
            geometryVector3.z = vector3.z;
            return geometryVector3;
        }

    }
}