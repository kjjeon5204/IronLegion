using UnityEngine;
using System.Collections;

public class ArmoryFrameButton : MonoBehaviour {
    public GameObject currentFrame;
    public ArmoryControl armoryControl;

    void Clicked()
    {
        armoryControl.disable_all_frame();
        currentFrame.SetActive(true);
    }
}
