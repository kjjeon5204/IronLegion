using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangerEquipmentControls : MonoBehaviour {
    public ItemDictionary itemDictionary;
    public HangerEquipInfoTab currentlyEquippedItemData;
    public HangerEquipInfoTab currentlySelectedItemData;
    public HangerPlayerStatus playerStatusDisplay;

    public UI2DController equipUIController;
    public UI2DController equipUIConfirmController;

    Item currentlySelectedItem;
    int curInventoryAccSlot;
    int currentEquipSlot; //Only used in the case that item is a Core

    public GameObject itemSlotSelectedMarker;

    public HangerEquipSlots[] equipmentSlots;
    int curSelEquipSlot;
    //Handles equipping item logic. This function is called when the equip button
    //is pressed by HangerEquipItemButton class
    //This function assumes that curSelEquipSlot and currently selected item is not null
    //If not, it doesn't do any action and displays user message
    public void equip_item_logic()
    {
        if (currentlySelectedItem != null)
        {
            //Handle UI Data
            Item itemUnequipped = equipmentSlots[curSelEquipSlot].set_equipment_window(currentlySelectedItem);
            if (itemUnequipped != null)
            {
                itemList[curInventoryAccSlot] = itemUnequipped.itemID;
                remove_item(itemUnequipped, curSelEquipSlot);
            }
            else
            {
                itemList.RemoveAt(curInventoryAccSlot);
            }
            equip_item(currentlySelectedItem, curSelEquipSlot);
            currentlySelectedItemData.set_item_stat(Item.nullItem);
            currentlyEquippedItemData.set_item_stat(Item.nullItem);
            playerStatusDisplay.update_player_data();
            populate_item_table();
            currentlySelectedItem = null;
        }
    }

    //Used to remove item. Checks to make sure removed item matches the item slot.
    //If there is a match, item is removed and stats are calculated accordingly.
    void remove_item(Item item, int itemSlot)
    {
        PlayerMechData currentMech = UserData.userDataContainer.get_current_mech();
        if (currentMech.itemsEquipped[itemSlot] == item.name)
        {
            //Compare item to make sure correct item is unequipped
            currentMech.itemsEquipped[itemSlot] = "000000";
            currentMech.health -= item.hp;
            currentMech.armor -= item.armor;
            currentMech.damage -= item.damage;
            currentMech.pentration -= item.penetration;
            currentMech.luck -= item.luck;
            currentMech.energy -= item.energy;
            UserData.userDataContainer.set_current_mech(currentMech);
        }
    }

    //Used to equip item. It checks to make sure item Slot specified
    //matches the item type. If there is a match, item is equipped.
    //Returns true when item has successfully been equipped. Otherwise
    //returns false.
    bool equip_item(Item item, int itemSlot)
    {
        PlayerMechData currentMech = UserData.userDataContainer.get_current_mech();
        if ((int)item.itemType == itemSlot || 
            (item.itemType == Item.ItemType.CORE && itemSlot == 3) ||
            (item.itemType == Item.ItemType.CORE && itemSlot == 4))
        {
            if (currentMech.itemsEquipped[itemSlot] == "000000")
            {
                currentMech.itemsEquipped[itemSlot] = item.name;
                currentMech.health += item.hp;
                currentMech.armor += item.armor;
                currentMech.damage += item.damage;
                currentMech.pentration += item.penetration;
                currentMech.luck += item.luck;
                currentMech.energy += item.energy;
                UserData.userDataContainer.set_current_mech(currentMech);
                return true;
            }
        }
        return false;
    }

    //Handles event where an equipment slot is pressed
    public void on_equip_slot_press(Item.ItemType equipItemType, Item itemEquipped)
    {
        if (currentlySelectedItem == null || equipItemType == currentlySelectedItem.itemType)
        {
            //if equipped item equals item type, no changes
            if (itemEquipped != null && itemEquipped.itemID != "000000")
            {
                currentlyEquippedItemData.set_item_stat(itemEquipped);
            }
            else
            {
                currentlyEquippedItemData.set_item_stat(Item.nullItem);
            }
        }
        else
        {
            currentlySelectedItem = null;
            currentlySelectedItemData.set_item_stat(Item.nullItem);
        }
    }

    //Always run this first time menu is initialized in order to 
    //apply any item update/patches
    void recalculate_player_item_stats()
    {
        PlayerMechData currentMech = UserData.userDataContainer.get_current_mech();
        currentMech.health = 0;
        currentMech.luck = 0;
        currentMech.armor = 0;
        currentMech.damage = 0;
        currentMech.pentration = 0;
        currentMech.energy = 0;
        for (int ctr = 0; ctr < 5; ctr++)
        {
            Item item = itemDictionary.get_item_data(currentMech.
                itemsEquipped[ctr]).GetComponent<Item>();

            currentMech.health += item.hp;
            currentMech.armor += item.armor;
            currentMech.damage += item.damage;
            currentMech.pentration += item.penetration;
            currentMech.luck += item.luck;
            currentMech.energy += item.energy;
        }
        UserData.userDataContainer.set_current_mech(currentMech);
    }

    
    public GameObject itemSlotPrefab;
    public GameObject inventoryItemPosition;
    public float xOffSet;
    public float yOffSet;
    IList<HangerEquipItemSlots> itemSlotsList;

    //deletes all items in the table
    void empty_item_table()
    {
        if (itemSlotsList != null)
        {
            for (int ctr = 0; ctr < itemSlotsList.Count; ctr++)
            {
                Destroy(itemSlotsList[ctr].gameObject);
            }
        }
    } 

    IList<string> itemList;
    //Takes the items from player inventory and populates the item table.
    //Slots are created as the items are populated.
    void populate_item_table()
    {
        empty_item_table();
        itemList = UserData.userDataContainer.get_item_inventory();
        Vector3 currentItemSlotPosition = inventoryItemPosition.transform.position;
        itemSlotsList = new List<HangerEquipItemSlots>();
        for (int ctr = 0; ctr < itemList.Count; ctr++)
        {
            Item curItem = itemDictionary.get_item_data(itemList[ctr]).GetComponent<Item>();
            GameObject tempItemSlot = (GameObject)Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
            itemSlotsList.Add(tempItemSlot.GetComponent<HangerEquipItemSlots>());
            itemSlotsList[ctr].set_current_item(curItem, ctr,this);
            if (ctr % 2 == 0 && ctr != 0)
            {
                //even numbered slot after first slot
                currentItemSlotPosition.y += yOffSet;
                itemSlotsList[ctr].gameObject.transform.position = currentItemSlotPosition;
            }
            else if (ctr == 0)
            {
                //first slot
                itemSlotsList[ctr].gameObject.transform.position = currentItemSlotPosition;
            }
            else
            {
                //odd numbered slot
                Vector3 tempPosition = currentItemSlotPosition;
                tempPosition.x += xOffSet;
                itemSlotsList[ctr].gameObject.transform.position = tempPosition;
            }
            itemSlotsList[ctr].transform.parent = inventoryItemPosition.transform;
        }
    }

    //Populates the item inventory by only items of certain type.
    public void populate_item_table(Item.ItemType itemType)
    {
        empty_item_table();
        int itemCtr = 0;
        IList<string>tempItemList = UserData.userDataContainer.get_item_inventory();
        Vector3 currentItemSlotPosition = inventoryItemPosition.transform.position;
        for (int ctr = 0; ctr < tempItemList.Count; ctr++)
        {
            Item curItem = itemDictionary.get_item_data(tempItemList[ctr]).GetComponent<Item>();
            if (curItem.itemType == itemType)
            {
                GameObject tempItemSlot = (GameObject)Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
                itemSlotsList.Add(tempItemSlot.GetComponent<HangerEquipItemSlots>());


                if (ctr % 2 == 0 && ctr != 0)
                {
                    //even numbered slot after first slot
                    currentItemSlotPosition.y += yOffSet;
                    itemSlotsList[itemCtr].gameObject.transform.position = currentItemSlotPosition;
                }
                else if (ctr == 0)
                {
                    //first slot
                    itemSlotsList[itemCtr].gameObject.transform.position = currentItemSlotPosition;
                }
                else
                {
                    //odd numbered slot
                    Vector3 tempPosition = currentItemSlotPosition;
                    tempPosition.x += xOffSet;
                    itemSlotsList[itemCtr].gameObject.transform.position = tempPosition;
                }
                itemCtr ++;
            }
        }
    }

    //Used by item slots to select the correct slot
    public void item_selected(Item itemSelected, int itemSlot)
    {
        currentlySelectedItem = itemSelected;
        curInventoryAccSlot = itemSlot;
        if (itemSelected.itemType == Item.ItemType.HEAD)
        {
            curSelEquipSlot = 0;
        }
        else if (itemSelected.itemType == Item.ItemType.WEAPON)
        {
            curSelEquipSlot = 1;
        }
        else if (itemSelected.itemType == Item.ItemType.ARMOR)
        {
            curSelEquipSlot = 2;
        }
        else if (itemSelected.itemType == Item.ItemType.CORE)
        {
            curSelEquipSlot = 3;
        }
        currentlySelectedItemData.set_item_stat(itemSelected);
        Item currentlyEquippedItem = equipmentSlots[curSelEquipSlot].get_equipped_item();
        if (currentlyEquippedItem == null)
        {
            currentlyEquippedItemData.set_item_stat(Item.nullItem);
        }
        else
        {
            currentlyEquippedItemData.set_item_stat(currentlyEquippedItem);
        }
    }

    /*********************************
     ************Sell Item************
     *********************************/

    public GameObject hangerConfirmCam;
    

    /*This function initiates the sell confirmation. If there is no item selected, the selling process
     is halted and a message displaying that an item from inventory must be selected is displayed. Otherwise,
     confirmation screen asking player if they are sure they want to buy is displayed.*/
    public void initiate_item_sale()
    {

    }

    /*This function handles item sell confirmation. Item currently set to sell is removed from the
     inventory and corresponding credit is sent to player credit pool.*/
    public void confirm_item_sale()
    {

    }

    /*This function is used to notify hanger controls that the item selling process has been canceled
     and switch to initial equipment control state*/
    public void cancel_item_sale()
    {

    }
    

    void Start()
    {
        Item.initialize_null_item();
        #region DEBUG_REGION
        IList<string> temporaryEquipment = new List<string>();
        temporaryEquipment.Add("01B001");
        temporaryEquipment.Add("01B001");
        temporaryEquipment.Add("01B001");
        temporaryEquipment.Add("01H001");
        temporaryEquipment.Add("01C001");
        temporaryEquipment.Add("01B001");
        temporaryEquipment.Add("01C001");
        temporaryEquipment.Add("01B001");
        UserData.initialize_user_data();
        PlayerMechData dummyMachine = new PlayerMechData();
        dummyMachine.mechID = "StupidMachine";
        dummyMachine.level = 1;
        dummyMachine.curExp = 0;
        dummyMachine.health = 0;
        dummyMachine.damage = 0;
        dummyMachine.armor = 0;
        dummyMachine.pentration = 0;
        dummyMachine.luck = 0;
        dummyMachine.energy = 0;
        dummyMachine.itemsEquipped = new List<string>();
        dummyMachine.itemsEquipped.Add("000000");
        dummyMachine.itemsEquipped.Add("000000");
        dummyMachine.itemsEquipped.Add("000000");
        dummyMachine.itemsEquipped.Add("000000");
        dummyMachine.itemsEquipped.Add("000000");

        UserData.userDataContainer.create_new_user_data("testID", dummyMachine);
        UserData.userDataContainer.set_item_inventory(temporaryEquipment);
        #endregion

        hangerConfirmCam.transform.position = Camera.main.transform.position;
        populate_item_table();
    }
}
