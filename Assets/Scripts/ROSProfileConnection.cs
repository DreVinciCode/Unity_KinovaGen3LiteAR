using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Protocols;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ROSProfileConnection : MonoBehaviour
{
    public TMP_Text IPText;
    private RosConnector _rosConnector;
    private string _deviceIP;
    public bool ConnectionCheck { get; set; }

    public UnityEvent RosConnectionEstablished;
    public UnityEvent RosConnectionFailed;

    private void Start()
    {
        ConnectionCheck = false;
        _rosConnector = GetComponent<RosConnector>();
        _rosConnector.Serializer = RosSocket.SerializerEnum.Newtonsoft_JSON;
        _rosConnector.protocol = Protocol.WebSocketSharp;
    }

    public void SetIPAddress()
    {
        _deviceIP = IPText.text;
        _rosConnector.RosBridgeServerUrl = "ws://" + _deviceIP + ":9090";
        _rosConnector.RosConnect();
    }

    private void Update()
    {
        if(ConnectionCheck)
        {
            if(_rosConnector.ConnectionStatus)
            {
                RosConnectionEstablished.Invoke();
                ConnectionCheck = false;
            }
            else
            {
                RosConnectionFailed.Invoke();
            }
        }
    }

}