using UnityEngine;

public class CirclePoints : Singleton<CirclePoints>
{
    public int segmentCount = 50; // 圆上的点数
    public float radius = 1000f; // 圆的半径

    // 获取圆圈点的方法
    public Vector3[] GetCirclePoints()
    {
        Vector3[] positions = new Vector3[segmentCount]; // 用于存储生成的点

        // 计算每个点的位置
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = 2 * Mathf.PI * i / segmentCount; // 计算角度，范围从0到2π
            float x = Mathf.Cos(angle) * radius; // 计算x坐标
            float y = Mathf.Sin(angle) * radius; // 计算y坐标
            positions[i] = new Vector3(x, 0, y); // 创建3D点并存储
        }

        return positions; // 返回生成的点数组
    }
}
