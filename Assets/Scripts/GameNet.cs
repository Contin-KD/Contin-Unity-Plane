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
        // 初始化服务器Socket
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
        serverSocket.Listen(10);
        Debug.Log("等待客户端链接...");

        // 开始异步接受客户端连接
        serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    public void AcceptCallback(IAsyncResult ar)
    {
        // 结束异步接受连接，并获取客户端Socket
        Socket clientSocket = serverSocket.EndAccept(ar);
        clientSockets.Add(clientSocket);
        Debug.Log("客户端连接成功");
        isConnected = true;

        // 继续接受其他客户端的连接
        serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    void OnApplicationQuit()
    {
        // 关闭所有客户端Socket
        foreach (Socket clientSocket in clientSockets)
        {
            clientSocket.Close();
        }
        // 关闭服务器Socket
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
