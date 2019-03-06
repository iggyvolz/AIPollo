using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private TextMeshPro captions;
    private TcpClient tcp;
    private StreamReader stream;
    private string text;
    [SerializeField]
    private Transform textBackground;
	// Use this for initialization
	void Start () {

        tcp= new TcpClient("127.0.0.1",7001);
        stream = new StreamReader(tcp.GetStream());
        new Thread(() =>
        {
            while (tcp.Connected)
            {
                text=stream.ReadLine();
            }
        }).Start();
    }

    // Update is called once per frame
    void Update ()
    {
        captions.text = text;
        captions.gameObject.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(text));
        Vector2 size = captions.GetRenderedValues();
        textBackground.localScale = new Vector3(size.x / 10 + 0.1f, 1, size.y / 10 + 0.1f);
    }
}
