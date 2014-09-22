using UnityEngine;
using System.Collections;

public class DeactivateObjectButton : MonoBehaviour {
    public GameObject[] deactivatedObject;

    void Clicked()
    {
        foreach (GameObject objectToTurnOff in deactivatedObject)
            objectToTurnOff.SetActive(false);
    }
}
