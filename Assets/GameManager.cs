using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private TextMeshPro captions;

    public string serverIP = "192.168.1.8";
    public int connectPort = 60001;

    private byte[] msgdata = new byte[1024];
    private NetworkStream stream;
     
    private UdpClient udp;
    private IPEndPoint remoteEP;

    private TcpClient tcp;

    [SerializeField]
    private Transform textBackground;
	// Use this for initialization
	void Start () {
        connectToCueServer();

        //udp = new UdpClient(receiverPort);
        //remoteEP = new IPEndPoint(IPAddress.Parse(serverIP), receiverPort);
        //remoteEP = new IPEndPoint(IPAddress.Broadcast, 0);
        // remoteEP = new IPEndPoint(IPAddress.Any, 0);
    }

    void connectToCueServer()
    {
    
        // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            tcp = new TcpClient(serverIP, connectPort);

            // Translate the passed message into ASCII and store it as a Byte array.
            byte[] data = System.Text.Encoding.ASCII.GetBytes("Connect Me");

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            stream = tcp.GetStream();

        // Send the message to the connected TcpServer. 
        stream.Write(data, 0, data.Length);
    }

    // Send a connect message to the server
    //void connectToCueServer()
    //{
        //UdpClient conClient = new UdpClient();
        //IPEndPoint conEP = new IPEndPoint(IPAddress.Parse(serverIP), connectPort);
        //conClient.Connect(conEP);

        // send the stupid connect message
        //System.Text.Encoding utf8 = Encoding.UTF8;
        //byte[] data = utf8.GetBytes("Connect Me");
        //conClient.Send(data, data.Length, conEP);
    //}

    // Receive the TcpServer.response.
    string getNextCaption()
    {
        // Read the first batch of the TcpServer response bytes.
        int bytes = stream.Read(msgdata, 0, msgdata.Length);
        string responseData = System.Text.Encoding.ASCII.GetString(msgdata, 0, bytes);

        return responseData;
    }

    // Update is called once per frame
    void Update ()
    {
        if (tcp.Available > 0)
        {
            //string text = System.Text.Encoding.Default.GetString(udp.Receive(ref remoteEP));
            string text = getNextCaption();
            captions.text = text;
            Debug.Log("text = " + text);
        }
        Vector2 size = captions.GetRenderedValues();
        // Debug.Log("Foo");
        textBackground.localScale = new Vector3(size.x / 10 + 0.1f, 1, size.y / 10 + 0.1f);
    }
}
