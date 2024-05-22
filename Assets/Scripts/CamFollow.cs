using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target; // Ŀ�����

    private void LateUpdate()
    {
        // ����Ŀ��λ��
        Vector3 targetPos = target.position + target.up * 10 + target.forward * -20;
        // ʹ��Lerp��������ƽ����ֵ
        Vector3 pos = Vector3.Lerp(transform.position, targetPos, 0.0125f);
        // �������λ��
        transform.position = pos;

        // ȷ�����ʼ������Ŀ��
        transform.LookAt(target);
    }
}
