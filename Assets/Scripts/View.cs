using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    public Button mode1Btn;
    public Button mode2Btn;
    // Start is called before the first frame update
    void Start()
    {
        mode1Btn.onClick.AddListener(() =>
        {
            PlayerController.instance.SetFlyState(State.Mode2);
        });
        mode2Btn.onClick.AddListener(() =>
        {
            PlayerController.instance.SetFlyState(State.Mode1);
        });
    }
}
