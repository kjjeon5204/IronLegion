using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class googleIABTestScript : BaseUIButton {
    public TextMesh textDisplay;
    GoogleIABBackend googleIABTest;

    public override void button_released_action()
    {
        Debug.Log("Touch activated!");
        textDisplay.text = "Start IAB initialization process!/n";
        googleIABTest = new GoogleIABBackend();
        textDisplay.text += "Starting IABInit/n";
        googleIABTest.initialize_IAB();
        /*
        //textDisplay.text += googleIABTest.get_current_IABState() + "/n";
        textDisplay.text = "IAB Initialized!/n";
        string[] itemList = { "test_item_1", "test_item2", "test_item3" };
        googleIABTest.initialize_skus(itemList);
        List<GoogleSkuInfo> itemAvailableList = googleIABTest.get_sku_data();
        if (itemAvailableList != null)
        {
            for (int ctr = 0; ctr < itemAvailableList.Count; ctr++)
            {
                textDisplay.text += itemAvailableList[ctr].title + "/n";
            }
        }
        else
        {
            textDisplay.text = "Failed to initialize!";
        }
         */ 
    }
}
