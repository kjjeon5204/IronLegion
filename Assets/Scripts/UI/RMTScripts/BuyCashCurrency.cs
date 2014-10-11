using UnityEngine;
using System.Collections;

public class BuyCashCurrency : CustomButton {
    public RMTMasterControls rmtMaster;

    public string productID;
    public int cogentumCtr;
    float price;

    public override void button_pressed()
    {
        rmtMaster.purchase_item(productID);
    }
}
