using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Mode1,
    Mode2,
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    // 当前的索引
    public int currentIndex = 1;

    public State flyState;

    public float moveSpeed = 10f;

    public bool isAddSpeed = true;

    public Vector3 target;

    public List<Vector3> mapPoint = new List<Vector3>();

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 设置飞行状态
    /// 导航 / 手动 
    /// </summary>
    /// <param name="mode"></param>
    public void SetFlyState(State mode)
    {
        flyState = mode;
        switch (flyState)
        {
            case State.Mode1:
                moveSpeed = 10;
                break;
            case State.Mode2:
                moveSpeed = 0.1f;
                FindNearestPoint();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 找到最近的点
    /// </summary>
    private void FindNearestPoint()
    {
        float min = Vector3.Distance(transform.position, mapPoint[0]);
        for (int i = 0; i < mapPoint.Count; i++)
        {
            float dis = Vector3.Distance(transform.position, mapPoint[i]);
            if (dis <= min)
            {
                min = dis;
                currentIndex = i + 1;
            }
        }
        target = mapPoint[currentIndex];
    }

    // Update is called once per frame
    void Update()
    {
        switch (flyState)
        {
            case State.Mode1:
                Move();
                SetHorizontal();
                SetVertical();
                ChangeSpeed();
                break;
            case State.Mode2:
                NavMove();
                break;
            default:
                break;
        }
        ServerSendMessageToClient();
    }

    private void ServerSendMessageToClient()
    {
        if (GameNet.Instance.isConnected)
        {
            PosMessage msg = new PosMessage
            {
                x = transform.position.x,
                y = transform.position.y,
                z = transform.position.z,
                rox = transform.localEulerAngles.x,
                roy = transform.localEulerAngles.y,
                roz = transform.localEulerAngles.z,
            };
            string response = JsonConvert.SerializeObject(msg);
            response += "\n"; // 添加换行符作为消息结束标志
            byte[] data = Encoding.UTF8.GetBytes(response);
            if (GameNet.Instance.clientSockets.Count > 0)
            {
                foreach (var item in GameNet.Instance.clientSockets)
                {
                    item.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
                }
            }

        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndSend(ar);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    
    // 导航移动
    private void NavMove()
    {
        if (mapPoint.Count <= 0)
        {
            Debug.Log("路径为空");
            return;
        }
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion tarQuat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarQuat, Time.deltaTime * 1);
        }
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed);
        float dis = Vector3.Distance(transform.position, target);
        if (dis <= 1)
        {
            currentIndex++;
            if (currentIndex >= mapPoint.Count)
            {
                currentIndex = 0;
            }
            target = mapPoint[currentIndex];
        }
    }

    /// <summary>
    /// 加减速
    /// </summary>
    private void ChangeSpeed()
    {
        if (isAddSpeed == false && moveSpeed >= 10)
        {
            moveSpeed -= 0.015f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isAddSpeed = true;
            moveSpeed += 0.01f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isAddSpeed = false;
        }
    }

    /// <summary>
    /// 手控移动
    /// </summary>
    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// 爬升跟低头
    /// </summary>
    private void SetVertical()
    {
        float v = Input.GetAxis("Vertical");
        if (v != 0)
        {
            transform.localEulerAngles += new Vector3(v / 10, 0, 0);
            float currentX = transform.localEulerAngles.x;
            if (currentX > 180f)
            {
                currentX -= 360f;
            }
            float clampX = Mathf.Clamp(currentX, -30, 30);
            if (clampX < 0f)
            {
                clampX += 360f;
            }

            transform.localEulerAngles = new Vector3(clampX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    /// <summary>
    /// 设置横滚
    /// </summary>
    private void SetHorizontal()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            transform.localEulerAngles -= new Vector3(0, -h / 15, h / 10);
            float currentZ = transform.localEulerAngles.z;
            if (currentZ > 180f)
            {
                currentZ -= 360f;
            }
            float clampZ = Mathf.Clamp(currentZ, -30, 30);
            if (clampZ < 0f)
            {
                clampZ += 360f;
            }
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, clampZ);
        }
    }

    /// <summary>
    /// 初始化路径点
    /// </summary>
    /// <param name="loadPoint"></param>
    internal void InitMapPoint(List<Vector3> loadPoint)
    {
        foreach (var point in loadPoint)
        {
            mapPoint.Add(point);
        }
        flyState = State.Mode2;
        switch (flyState)
        {
            case State.Mode1:
                moveSpeed = 10;
                break;
            case State.Mode2:
                moveSpeed = 0.1f;
                target = mapPoint[currentIndex];
                break;
            default:
                break;
        }
    }
}
