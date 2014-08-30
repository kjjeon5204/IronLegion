using UnityEngine;
using System.Collections;


public class ItemDisplayWindow : MonoBehaviour {
    public enum SlotType
    {
        RANDOM,
        UNLOCKED,
        LOCKED
    }

    public struct ItemSlot
    {
        public SlotType slotType;
        public GameObject itemSlot;
    }
    ItemSlot[] itemSlots;
    public GameObject randomObjectSlot;
    public GameObject lockedSlotFrame;
    public GameObject itemSlotFrame;
    StoreData myStoreData;
    int numOfOpenSpot = 3;
    public float verticalSlotOffset;
    public GameObject slotPosition;
    public GameObject itemDictionary;
    public ArmoryControl armoryControl;
    public ArmoryDataControl armoryData;
    public GameObject itemDisplayFrame;


    int creditOwned;
    int cogentumOwned;

    


    public void initialize_store_data(StoreData inputInventory, ArmoryControl inArmoryControl)
    {
        armoryControl = inArmoryControl;
        myStoreData = inputInventory;
        itemSlots = new ItemSlot[4];
        Debug.Log("Number of items in store: " + inputInventory.soldItemList.Count);
        itemSlots[0].slotType = SlotType.RANDOM;
        itemSlots[0].itemSlot = (GameObject)Instantiate(randomObjectSlot, slotPosition.transform.position, Quaternion.identity);
        itemSlots[0].itemSlot.transform.parent = slotPosition.transform;
        for (int ctr = 1; ctr < numOfOpenSpot; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.y -= ctr * verticalSlotOffset;
            itemSlots[ctr].slotType = SlotType.UNLOCKED;
            itemSlots[ctr].itemSlot = (GameObject)Instantiate(itemSlotFrame, windowPosition, Quaternion.identity);
            itemSlots[ctr].itemSlot.GetComponent<ItemSlotWindow>().
                set_item_slot(inputInventory.soldItemList[ctr], inArmoryControl, ctr);
            itemSlots[ctr].itemSlot.transform.parent = slotPosition.transform;
        }
        for (int ctr = numOfOpenSpot; ctr < itemSlots.Length; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.y -= ctr * verticalSlotOffset;
            itemSlots[ctr].slotType = SlotType.LOCKED;
            itemSlots[ctr].itemSlot = (GameObject)Instantiate(lockedSlotFrame, windowPosition, Quaternion.identity);
            itemSlots[ctr].itemSlot.transform.parent = slotPosition.transform;
        }
        armoryData.save_store_data(myStoreData);
    }

    public void slot_unlocked_info(int slotAccess)
    {
        GameObject tempHolder = itemSlots[slotAccess].itemSlot;
        itemSlots[slotAccess].slotType = SlotType.UNLOCKED;
        itemSlots[slotAccess].itemSlot = (GameObject)Instantiate(itemSlotFrame,
            itemSlots[slotAccess].itemSlot.transform.position, Quaternion.identity);
        Destroy(tempHolder);
        numOfOpenSpot++;
    }

    public void currency_update(int credit, int cogentum)
    {
        creditOwned = credit;
        cogentumOwned = cogentum;
        for (int ctr = 0; ctr < itemSlots.Length; ctr++)
        {
            if (itemSlots[ctr].slotType == SlotType.UNLOCKED)
            {
                itemSlots[ctr].itemSlot.GetComponent<ItemSlotWindow>().
                    currency_update(credit, cogentum);
            }
        }
    }

    public void currency_update(int credit, int cogentum, int slotNum)
    {
        creditOwned = credit;
        cogentumOwned = cogentum;
        for (int ctr = 0; ctr < itemSlots.Length; ctr++)
        {
            if (itemSlots[ctr].slotType == SlotType.UNLOCKED)
            {
                itemSlots[ctr].itemSlot.GetComponent<ItemSlotWindow>().
                    currency_update(credit, cogentum);
            }
        }
        item_bought(slotNum);
        armoryData.save_store_data(myStoreData);
    }

    public void item_bought(int slotAccess)
    {
        ArmoryCatalog tempCatalog = myStoreData.soldItemList[slotAccess];
        tempCatalog.itemSaleStatus = true;
        itemSlots[slotAccess].itemSlot.GetComponent<ItemSlotWindow>().
            update_sale_status(tempCatalog.itemSaleStatus);
        myStoreData.soldItemList[slotAccess] = tempCatalog;
    }


    public int get_unlocked_slot_count()
    {
        return numOfOpenSpot;
    }

    void touch_parser(Touch curTouch)
    {
        Vector2 curTouchPos = Camera.main.ScreenToWorldPoint(curTouch.position);
        LayerMask myLayerMask = LayerMask.NameToLayer("ItemSlotWindow");
        RaycastHit2D curRayCast = Physics2D.Raycast(curTouchPos, Vector2.zero, 100.0f, (int)myLayerMask);
        if (curRayCast != null)
        {
            if (curRayCast.collider.gameObject.tag == "ItemSlotWindow")
            {
                if (curTouch.deltaPosition.y > 0.0f)
                {
                    slotPosition.transform.Translate(Vector3.up * 1.0f * Time.deltaTime);
                }
                if (curTouch.deltaPosition.y < 0.0f)
                {
                    slotPosition.transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
                }
            }
        }
    }

    void Update()
    {
        /*
        foreach (Touch curTouch in Input.touches)
            touch_parser(curTouch);
         */ 
    }
}
