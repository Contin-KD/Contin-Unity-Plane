using UnityEngine;

public class CirclePoints : Singleton<CirclePoints>
{
    public int segmentCount = 50; // Բ�ϵĵ���
    public float radius = 1000f; // Բ�İ뾶

    // ��ȡԲȦ��ķ���
    public Vector3[] GetCirclePoints()
    {
        Vector3[] positions = new Vector3[segmentCount]; // ���ڴ洢���ɵĵ�

        // ����ÿ�����λ��
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = 2 * Mathf.PI * i / segmentCount; // ����Ƕȣ���Χ��0��2��
            float x = Mathf.Cos(angle) * radius; // ����x����
            float y = Mathf.Sin(angle) * radius; // ����y����
            positions[i] = new Vector3(x, 0, y); // ����3D�㲢�洢
        }

        return positions; // �������ɵĵ�����
    }
}
