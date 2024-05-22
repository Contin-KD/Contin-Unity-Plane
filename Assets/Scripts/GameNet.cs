using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class GameNet : Singleton<GameNet>
{
    private Socket serverSocket;
    public List<Socket> clientSockets = new List<Socket>();
    private byte[] buffer = new byte[1024];
    public bool isConnected = false;

    public void Start()
    {
        // ��ʼ��������Socket
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
        serverSocket.Listen(10);
        Debug.Log("�ȴ��ͻ�������...");

        // ��ʼ�첽���ܿͻ�������
        serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    public void AcceptCallback(IAsyncResult ar)
    {
        // �����첽�������ӣ�����ȡ�ͻ���Socket
        Socket clientSocket = serverSocket.EndAccept(ar);
        clientSockets.Add(clientSocket);
        Debug.Log("�ͻ������ӳɹ�");
        isConnected = true;

        // �������������ͻ��˵�����
        serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    void OnApplicationQuit()
    {
        // �ر����пͻ���Socket
        foreach (Socket clientSocket in clientSockets)
        {
            clientSocket.Close();
        }
        // �رշ�����Socket
        serverSocket.Close();
    }
}
public class PosMessage
{
    public float x;
    public float y;
    public float z;
    public float rox;
    public float roy;
    public float roz;
}
