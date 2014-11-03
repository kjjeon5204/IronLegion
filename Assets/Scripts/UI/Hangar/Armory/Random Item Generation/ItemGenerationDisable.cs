using UnityEngine;
using System.Collections;

public class ItemGenerationDisable : MonoBehaviour {
    public GameObject itemGenWindow;
    public GameObject itemDisplayWindow;


    void Clicked()
    {
        itemGenWindow.gameObject.SetActive(false);
        itemDisplayWindow.gameObject.SetActive(false);
    }
}
