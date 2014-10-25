using UnityEngine;
using System.Collections;

public class ItemBuyConfirmButton : MonoBehaviour {
    Item genItem;
    public GameObject[] windowsToBeDisabled;
    public Renderer textMeshWindow;
    public string sortingLayerName;
    public PlayerMasterData playerMasterData;


    public void set_confirmation_button(Item generatedItem)
    {
        genItem = generatedItem;
    }

    void Start()
    {
        textMeshWindow.sortingLayerName = sortingLayerName;
    }

    void Clicked ()
    {
        //place item into inventory
        /*
        Inventory playerInventory = new Inventory();
        playerInventory.load_inventory();
        playerInventory.add_item(genItem.itemID);
        playerInventory.store_inventory();
         */
        playerMasterData.add_item(genItem.itemID);
        foreach (GameObject windowAcc in windowsToBeDisabled)
        {
            windowAcc.SetActive(false);
        }
    }
}
