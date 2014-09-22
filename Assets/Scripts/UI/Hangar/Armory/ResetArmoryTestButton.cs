using UnityEngine;
using System.Collections;

public class ResetArmoryTestButton : MonoBehaviour {
    public ArmoryControl armoryController;

    void Clicked()
    {
        armoryController.reset_catalog_data();
    }
}
