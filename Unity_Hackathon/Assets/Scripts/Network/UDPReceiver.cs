// UDPReceiver.cs
// UDPでy座標（str型）を受信し、最新データを保持する
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    [Header("UDP受信ポート番号")]
    public int port = 5005;

    // 最新の受信y座標（文字列、外部から参照可能）
    [NonSerialized]
    public string latestYString = "";

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool running = false;

    void Start()
    {
        // UDP受信スレッド開始
        udpClient = new UdpClient(port);
        running = true;
        receiveThread = new Thread(new ThreadStart(ReceiveLoop));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // UDP受信ループ（別スレッド）
    private void ReceiveLoop()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (running)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string yStr = Encoding.UTF8.GetString(data);
                latestYString = yStr;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"UDP受信エラー: {e.Message}");
            }
        }
    }

    // アプリ終了時にスレッドとUDPクライアントを停止
    void OnApplicationQuit()
    {
        running = false;
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(100);
            receiveThread = null;
        }
    }
}
