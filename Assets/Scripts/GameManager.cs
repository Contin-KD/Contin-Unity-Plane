using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Vector3> loadPoint = new List<Vector3>();
    public Transform load;
    public GameObject plane;
    private void Awake()
    {
        GameNet.Instance.Start();
    }
    // Start is called before the first frame update
    void Start()
    {
        loadPoint = CirclePoints.Instance.GetCirclePoints().ToList();
        for (int i = 0; i < loadPoint.Count; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = loadPoint[i];
            go.transform.SetParent(load);
            go.name = i.ToString();
        }
        // 一开始就在第一个点位
        plane.transform.position = loadPoint[0];
        plane.GetComponent<PlayerController>().InitMapPoint(loadPoint);
    }
}
