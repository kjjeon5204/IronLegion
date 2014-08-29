using UnityEngine;
using System.Collections;

public class ArmoryTabButton : MonoBehaviour {
    public GameObject windowAccess;

    public void window_switch(bool switchCondition)
    {
        windowAccess.SetActive(switchCondition);
    }
}
