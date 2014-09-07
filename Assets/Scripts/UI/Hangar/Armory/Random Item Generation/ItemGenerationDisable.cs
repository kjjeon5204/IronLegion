using UnityEngine;
using System.Collections;

public class ItemGenerationDisable : MonoBehaviour {
    public ItemWindowDisplay itemGenWindow;
    public ItemGeneratedWindow itemDisplayWindow;


    void Clicked()
    {
        itemGenWindow.gameObject.SetActive(false);
        itemDisplayWindow.gameObject.SetActive(false);
    }
}
