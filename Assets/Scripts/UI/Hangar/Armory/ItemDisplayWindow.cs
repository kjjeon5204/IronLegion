using UnityEngine;
using System.Collections;

public class ItemDisplayWindow : MonoBehaviour {
    GameObject[] itemSlot;
    public GameObject randomObjectSlot;
    public GameObject lockedSlotFrame;
    public GameObject itemSlotFrame;
    StoreData myStoreData;
    int numOfOpenSpot;
    public float verticalSlotOffset;
    public GameObject slotPosition;

    int creditOwned;
    int cogentumOwned;

    public TextMesh cogentumOwnedDisplay;
    public TextMesh creditOwnedDisplay;

    public void initialize_store_data(StoreData inputInventory)
    {
        myStoreData = inputInventory;
        for (int ctr = 0; ctr < numOfOpenSpot; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.z -= ctr * verticalSlotOffset;
            itemSlot[ctr] = (GameObject)Instantiate(itemSlotFrame, windowPosition, Quaternion.identity);
            itemSlot[ctr].GetComponent<ItemSlotWindow>().set_item_slot(inputInventory.soldItemList[ctr]);
        }
        for (int ctr = numOfOpenSpot - 1; ctr < itemSlot.Length; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.z -= ctr * verticalSlotOffset;
            itemSlot[ctr] = (GameObject)Instantiate(lockedSlotFrame, windowPosition, Quaternion.identity);
        }

        Inventory playerInventory = new Inventory();
        playerInventory.load_inventory();
        creditOwned = playerInventory.get_currency();
        creditOwnedDisplay.text = creditOwned.ToString();

        cogentumOwned = playerInventory.get_paid_currency();
        cogentumOwnedDisplay.text = cogentumOwned.ToString();
    }
}
