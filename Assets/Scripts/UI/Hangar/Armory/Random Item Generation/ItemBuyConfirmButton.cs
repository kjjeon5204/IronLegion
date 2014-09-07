using UnityEngine;
using System.Collections;

public class ItemBuyConfirmButton : MonoBehaviour {
    Item genItem;
    public GameObject[] windowsToBeDisabled;

    public void set_confirmation_button(Item generatedItem)
    {
        genItem = generatedItem;
    }

    void Clicked ()
    {
        foreach (GameObject windowAcc in windowsToBeDisabled) 
        {
            windowAcc.SetActive(false);
        }
        //place item into inventory
        Inventory playerInventory = new Inventory();
        playerInventory.load_inventory();
        playerInventory.add_item(genItem.itemID);
        playerInventory.store_inventory();
    }
}
