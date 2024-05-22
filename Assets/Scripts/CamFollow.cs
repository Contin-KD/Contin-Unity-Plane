using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target; // 目标对象

    private void LateUpdate()
    {
        // 计算目标位置
        Vector3 targetPos = target.position + target.up * 10 + target.forward * -20;
        // 使用Lerp函数进行平滑插值
        Vector3 pos = Vector3.Lerp(transform.position, targetPos, 0.0125f);
        // 设置相机位置
        transform.position = pos;

        // 确保相机始终面向目标
        transform.LookAt(target);
    }
}
