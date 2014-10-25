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

    public GameObject upperLimit;
    public GameObject lowerLimit;
    public GameObject upperRefPt;
    public GameObject lowerRefPt;


    int creditOwned;
    int cogentumOwned;

    Vector2 previousMousePos;


    public void reinitialize_store_data(StoreData newStore)
    {
        for (int ctr = 0; ctr < itemSlots.Length; ctr++)
        {
            if (itemSlots[ctr].itemSlot != null)
                Destroy(itemSlots[ctr].itemSlot);
        }
        numOfOpenSpot = newStore.numberOfUnlockedSpot;
        myStoreData = newStore;
        itemSlots = new ItemSlot[4];
        
        for (int ctr = 1; ctr < numOfOpenSpot; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.y -= ctr * verticalSlotOffset;
            itemSlots[ctr].slotType = SlotType.UNLOCKED;
            itemSlots[ctr].itemSlot = (GameObject)Instantiate(itemSlotFrame, windowPosition, Quaternion.identity);
            itemSlots[ctr].itemSlot.GetComponent<ItemSlotWindow>().
                set_item_slot(newStore.soldItemList[ctr], armoryControl, ctr);
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
        Vector3 windowPos = slotPosition.transform.position;
        windowPos.y -= 4 * verticalSlotOffset;
        lowerRefPt.transform.position = windowPos;
        armoryData.save_store_data(myStoreData);
    }


    public void initialize_store_data(StoreData inputInventory, ArmoryControl inArmoryControl)
    {
        armoryControl = inArmoryControl;
        myStoreData = inputInventory;
        itemSlots = new ItemSlot[4];
        numOfOpenSpot = inputInventory.numberOfUnlockedSpot;
        
        for (int ctr = 1; ctr < numOfOpenSpot + 1; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.y -= ctr * verticalSlotOffset;
            itemSlots[ctr].slotType = SlotType.UNLOCKED;
            itemSlots[ctr].itemSlot = (GameObject)Instantiate(itemSlotFrame, windowPosition, Quaternion.identity);
            itemSlots[ctr].itemSlot.GetComponent<ItemSlotWindow>().
                set_item_slot(inputInventory.soldItemList[ctr], inArmoryControl, ctr);
            itemSlots[ctr].itemSlot.transform.parent = slotPosition.transform;
        }
        for (int ctr = numOfOpenSpot + 1; ctr < itemSlots.Length; ctr++)
        {
            Vector3 windowPosition = slotPosition.transform.position;
            windowPosition.y -= ctr * verticalSlotOffset;
            itemSlots[ctr].slotType = SlotType.LOCKED;
            itemSlots[ctr].itemSlot = (GameObject)Instantiate(lockedSlotFrame, windowPosition, Quaternion.identity);
            itemSlots[ctr].itemSlot.transform.parent = slotPosition.transform;
            itemSlots[ctr].itemSlot.GetComponent<LockedSlot>().unlockSlotButton.initialize_slot(armoryControl,
                ctr, 100, myStoreData.catalogType);
        }
        Vector3 windowPos = slotPosition.transform.position;
        windowPos.y -= 4 * verticalSlotOffset;
        lowerRefPt.transform.position = windowPos;
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

    public void unlock_slot(int credit, int cogentum)
    {
        creditOwned = credit;
        cogentumOwned = cogentum;

        //Process for unlocking slot
        GameObject temp = itemSlots[++numOfOpenSpot].itemSlot;
        itemSlots[numOfOpenSpot].itemSlot = (GameObject)Instantiate(itemSlotFrame,
            temp.transform.position, temp.transform.rotation);
        itemSlots[numOfOpenSpot].itemSlot.transform.parent = temp.transform.parent;
        itemSlots[numOfOpenSpot].itemSlot.GetComponent<ItemSlotWindow>().
            set_item_slot(myStoreData.soldItemList[numOfOpenSpot], armoryControl, numOfOpenSpot);
        myStoreData.numberOfUnlockedSpot = numOfOpenSpot;
        Destroy(temp);
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
        LayerMask myLayerMask = LayerMask.NameToLayer("ItemDisplayFrame");
        RaycastHit2D curRayCast = Physics2D.Raycast(curTouchPos, Vector2.zero, 100.0f, 1 << 20);
        if (curRayCast.collider != null)
        {
            if (curRayCast.collider.gameObject.tag == "ItemSlotWindow" && curTouch.phase == TouchPhase.Moved)
            {
                if (lowerRefPt.transform.position.y < lowerLimit.transform.position.y &&
                    curTouch.deltaPosition.y > 0.0f)
                {
                    slotPosition.transform.Translate(Vector3.up * 8.0f * Time.deltaTime);
                }
                if (upperRefPt.transform.position.y > upperLimit.transform.position.y &&
                    curTouch.deltaPosition.y < 0.0f)
                {
                    slotPosition.transform.Translate(Vector3.down * 8.0f * Time.deltaTime);
                }
            }
        }
    }

    

    void PC_input_parser()
    {
        Vector2 curTouchPos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LayerMask myLayerMask = LayerMask.NameToLayer("ItemDisplayFrame");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D curRayCast = Physics2D.Raycast(curTouchPos, Vector2.zero, 50.0f, 1<<20);
            Vector2 mouseDeltaPos = curTouchPos - previousMousePos;
            if (curRayCast.collider != null)
            {
                if (curRayCast.collider.gameObject.tag == "ItemSlotWindow" )
                {
                    Debug.Log("Run PC Input");
                    if (mouseDeltaPos.y > 0.0f)
                    {
                        slotPosition.transform.Translate(Vector3.up * 8.0f * Time.deltaTime);
                    }
                    if (mouseDeltaPos.y < 0.0f)
                    {
                        slotPosition.transform.Translate(Vector3.down * 8.0f * Time.deltaTime);
                    }
                }
            }
        }
        previousMousePos = curTouchPos;
    }

    void Update()
    {
        PC_input_parser();
        foreach (Touch curTouch in Input.touches)
            touch_parser(curTouch);
    }
}
