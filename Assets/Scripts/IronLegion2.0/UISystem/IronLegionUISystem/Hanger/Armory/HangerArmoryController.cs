using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangerArmoryStoreData
{
    IList<string>[] itemCatalogue = new IList<string>[4];

    public HangerArmoryStoreData (ItemDictionary itemDictionary)
    {
        //For temporary testing purposes
        reset_item_catalogue(5, itemDictionary);
    }

    void read_catalog_from_file()
    {
        //Load armory data from a file
    }

    void store_catalogue_state()
    {
        //Store armory data to a file
    }

    void reset_item_catalogue (int playerLevel, ItemDictionary itemDictionary)
    {
        itemDictionary.set_pooling_tier(playerLevel);
        //Head
        itemCatalogue[0] = new List<string>();
        for (int ctr = 0; ctr < 5; ctr++)
        {
            itemCatalogue[0].Add(itemDictionary.generate_random_item(Item.ItemType.HEAD).name);
        }
        //Weapon
        itemCatalogue[1] = new List<string>();
        for (int ctr = 0; ctr < 5; ctr++)
        {
            itemCatalogue[1].Add(itemDictionary.generate_random_item(Item.ItemType.WEAPON).name);
        }
        //Armor
        itemCatalogue[2] = new List<string>();
        for (int ctr = 0; ctr < 5; ctr++)
        {
            itemCatalogue[2].Add(itemDictionary.generate_random_item(Item.ItemType.ARMOR).name);
        }
        //Core
        itemCatalogue[3] = new List<string>();
        for (int ctr = 0; ctr < 5; ctr++)
        {
            itemCatalogue[3].Add(itemDictionary.generate_random_item(Item.ItemType.CORE).name);
        }
    }
}

public class HangerArmoryController : MonoBehaviour {
    public HangerItemComparisonWindow itemComparison;
    public ItemDictionary itemDictionary;
    public HangerArmoryBuyConfirm itemBuyConfirmWindow;

    //Input controllers
    public UI2DController uiOverlayControl;
    public UI2DController itemScrollControl;
    public UI2DController itemInfoBuyControl;
    public UI2DController confirmControl;
    public UI2DController randomBoxControl;

    

    //Starts the item buy transaction process
    public void initiate_transaction_process(Item itemToBuy)
    {
        itemBuyConfirmWindow.gameObject.SetActive(true);
    }

    public enum TransactionOption {
        EQUIP_ON_BUY,
        STORE_ON_BUY
    }

    //Completes the transaction and necessary cogentum and credit is subtracted
    //from current pool of user cogentum and credit. Also the item is added to 
    //inventory or equipped depending on the choice that player selected. EquipSlot
    //is only used in the case that the item is a core
    public void complete_trasaction_process(Item itemBought, TransactionOption transactionOption, int equipSlot)
    {
        UserData.userDataContainer.creditOwned -= itemBought.buy_price;
        UserData.userDataContainer.cogentumOwned -= itemBought.cg_price;
        if (transactionOption == TransactionOption.EQUIP_ON_BUY)
        {
            //Find item currently equipped
            Item itemCurEquipped = Item.nullItem;
            int itemSlotToCheck = -1; //If >= 0 then use this value to check slot otherwise, use equipSlot
            if (itemBought.itemType == Item.ItemType.HEAD)
            {
                itemSlotToCheck = 0;
            }
            else if (itemBought.itemType == Item.ItemType.WEAPON)
            {
                itemSlotToCheck = 1;
            }
            else if (itemBought.itemType == Item.ItemType.ARMOR)
            {
                itemSlotToCheck = 2;
            }
            else
            {
                itemSlotToCheck = equipSlot;
            }

            PlayerMechData curMech = UserData.userDataContainer.get_current_mech();
            itemCurEquipped = itemDictionary.get_item_data(curMech.itemsEquipped[itemSlotToCheck]).GetComponent<Item>();


            if (itemCurEquipped != null) //There is an item equipped
            {
                //Unequip item
                curMech.health -= itemCurEquipped.hp;
                curMech.armor -= itemCurEquipped.armor;
                curMech.damage -= itemCurEquipped.damage;
                curMech.pentration -= itemCurEquipped.penetration;
                curMech.energy -= itemCurEquipped.energy;
                curMech.luck -= itemCurEquipped.luck;

            }
            curMech.itemsEquipped[itemSlotToCheck] = itemBought.itemID;
            curMech.health += itemBought.hp;
            curMech.armor += itemBought.armor;
            curMech.damage += itemBought.damage;
            curMech.pentration += itemBought.penetration;
            curMech.energy += itemBought.energy;
            curMech.luck += itemBought.luck;
        }
        else
        {
            UserData.userDataContainer.add_item_to_inventory(itemBought.itemID);
        }
    }

    //Starts the slot unlock process
    public void initiate_unlock_window_process(HangerItemInformation hangerItemInfoWindow)
    {
        itemBuyConfirmWindow.set_unlock_slot_message(hangerItemInfoWindow, 200); // 0 is a temp value for now.
    }

    //Completes unlocking of a window
    public void complete_unlock_window(HangerItemInformation hangerArmoryItemWindow)
    {
        Item itemToInsert = Item.nullItem; //Later create data struct class to handle item input
        //unlock hanger armory window
        hangerArmoryItemWindow.initialize_item_window(itemToInsert, this);
        
        //subtract from player's credits
        UserData.userDataContainer.creditOwned -= 200; //temporary value

        //Finish transaction by updating all locked windows with new modified required price

    }

    public ItemInformationDisplay itemInfoDisplay;

    //Used to update item comparison window
    public void update_item_information_window(Item itemSelected)
    {
        
        int itemAccess = 0;
        if (itemSelected.itemType == Item.ItemType.HEAD) 
        {
            itemAccess = 0;
        }
        if (itemSelected.itemType == Item.ItemType.WEAPON) 
        {
            itemAccess = 1;
        }
        if (itemSelected.itemType == Item.ItemType.ARMOR) 
        {
            itemAccess = 2;
        }
        if (itemSelected.itemType == Item.ItemType.CORE) 
        {
            itemAccess = 3;
        }
        //string itemID = UserData.userDataContainer.get_current_mech().itemsEquipped[itemAccess];
        Item currentlyEquippedItem = null;
        statChangeDisplay.check_item_switch(itemSelected, null);
        //itemComparison.compare_stats(currentlyEquippedItem, itemSelected);
    }

    




    public ArmoryDataControl armoryStoreData;
    public GameObject hangerItemInfoPrefab;
    public Vector3 windowOffset;
    StoreData headStoreData;
    StoreData weaponStoreData;
    StoreData armorStoreData;
    StoreData coreStoreData;

    [System.Serializable]
    public struct ItemStoreFunctionObjects
    {
        public GameObject itemWindowHolder;
    }

    public ItemStoreFunctionObjects[] itemStoreObjects;
    HangerItemInformation[] headItemWindows;
    HangerItemInformation[] weaponItemWindows;
    HangerItemInformation[] armorItemWindows;
    HangerItemInformation[] coreItemWindows;

    //This function is used to populate all the store catalogs. This function is called under two conditions
    //1. When player first enters the armory during the duration of the scene.
    //2. When player hits armory refresh button.
    void populate_armory_table()
    {
        
        headStoreData = armoryStoreData.head_catalog_data();
        weaponStoreData = armoryStoreData.weapon_catalog_data();
        armorStoreData = armoryStoreData.armor_catalog_data();
        coreStoreData = armoryStoreData.core_catalog_data();
        int totalUnlockCount = headStoreData.numberOfUnlockedSpot + weaponStoreData.numberOfUnlockedSpot +
            armorStoreData.numberOfUnlockedSpot + coreStoreData.numberOfUnlockedSpot;
        //Fill table for each item type
        //Head
        #region HEAD
        headItemWindows = new HangerItemInformation[5];
        //Initialize unlocked windows
        #region UNLOCKED_WINDOW
        Vector3 windowPosition = itemStoreObjects[0].itemWindowHolder.transform.position;
        for (int ctr = 0; ctr < headStoreData.numberOfUnlockedSpot; ctr++)
        {
            GameObject tempInfoWindow = (GameObject)Instantiate(hangerItemInfoPrefab, 
                windowPosition, Quaternion.identity);
            HangerItemInformation tempInfoWindowAcc = tempInfoWindow.GetComponent<HangerItemInformation>();

            tempInfoWindow.transform.parent = itemStoreObjects[0].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindowAcc.initialize_item_window(itemDictionary.get_item_data(headStoreData.soldItemList[ctr].itemID)
                .GetComponent<Item>(), this);
            headItemWindows[ctr] = tempInfoWindowAcc;
        }
        #endregion
        //Initialize locked windows
        #region LOCKED_WINDOW
        for (int ctr = headStoreData.numberOfUnlockedSpot; ctr < headStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab,
                windowPosition, Quaternion.identity)).GetComponent<HangerItemInformation>();

            tempInfoWindow.transform.parent = itemStoreObjects[0].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.set_locked_item_window(totalUnlockCount * 1000, 0, Item.ItemType.HEAD,
                this) + 0.5f;
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        #endregion
        //Weapon
        #region WEAPON
        weaponItemWindows = new HangerItemInformation[5];
        //Initialize unlocked windows
        #region UNLOCKED_WINDOW
        windowPosition = itemStoreObjects[1].itemWindowHolder.transform.position;
        for (int ctr = 0; ctr < weaponStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab, 
                windowPosition, Quaternion.identity)).GetComponent<HangerItemInformation>();


            tempInfoWindow.transform.parent = itemStoreObjects[1].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.initialize_item_window(itemDictionary.get_item_data(weaponStoreData.soldItemList[ctr].itemID)
                .GetComponent<Item>(), this);
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        //Initialize locked windows
        #region LOCKED_WINDOW
        for (int ctr = weaponStoreData.numberOfUnlockedSpot; ctr < weaponStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab,
                itemStoreObjects[1].itemWindowHolder.transform.position - ctr * windowOffset
                , Quaternion.identity)).GetComponent<HangerItemInformation>();


            tempInfoWindow.transform.parent = itemStoreObjects[1].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.set_locked_item_window(totalUnlockCount * 1000, 0, Item.ItemType.WEAPON,
                this);
            tempInfoWindow.transform.parent = itemStoreObjects[1].itemWindowHolder.transform;
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        #endregion
        //Armor
        #region ARMOR
        armorItemWindows = new HangerItemInformation[5];
        //initialize unlocked windows
        #region UNLOCKED_WINDOWS
        windowPosition = itemStoreObjects[2].itemWindowHolder.transform.position;
        for (int ctr = 0; ctr < armorStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab, 
                windowPosition, Quaternion.identity)).GetComponent<HangerItemInformation>();


            tempInfoWindow.transform.parent = itemStoreObjects[2].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.initialize_item_window(itemDictionary.get_item_data(armorStoreData.soldItemList[ctr].itemID)
                .GetComponent<Item>(), this);
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        //initialize locked windows
        #region LOCKED_WINDOWS
        for (int ctr = armorStoreData.numberOfUnlockedSpot; ctr < armorStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab,
                windowPosition, Quaternion.identity)).GetComponent<HangerItemInformation>();


            tempInfoWindow.transform.parent = itemStoreObjects[2].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.set_locked_item_window(totalUnlockCount * 1000, 0, Item.ItemType.ARMOR,
                this);
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        #endregion
        //Core
        #region CORE
        coreItemWindows = new HangerItemInformation[5];
        //initialize unlocked windows
        #region UNLOCKED_WINDOW
        windowPosition = itemStoreObjects[3].itemWindowHolder.transform.position;
        for (int ctr = 0; ctr < coreStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab, 
                windowPosition, Quaternion.identity)).GetComponent<HangerItemInformation>();


            tempInfoWindow.transform.parent = itemStoreObjects[3].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.initialize_item_window(itemDictionary.get_item_data(coreStoreData.soldItemList[ctr].itemID)
                .GetComponent<Item>(), this);
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        //initialize locked windows
        #region LOCKED_WINDOW
        for (int ctr = coreStoreData.numberOfUnlockedSpot; ctr < coreStoreData.soldItemList.Count; ctr++)
        {
            HangerItemInformation tempInfoWindow = ((GameObject)Instantiate(hangerItemInfoPrefab,
                windowPosition, Quaternion.identity)).GetComponent<HangerItemInformation>();


            tempInfoWindow.transform.parent = itemStoreObjects[3].itemWindowHolder.transform;
            windowPosition.y = tempInfoWindow.set_locked_item_window(totalUnlockCount * 1000, 0, Item.ItemType.CORE,
                this);
            headItemWindows[ctr] = tempInfoWindow;
        }
        #endregion
        #endregion
    }

    //Displays the correct window based on the input. 
    //0 = Head
    //1 = Weapon
    //2 = Armor
    //3 = Core
    public void store_window_operator(int curWindow)
    {
        for (int ctr = 0; ctr < 4; ctr++)
        {
            if (ctr == curWindow)
            {
                itemStoreObjects[ctr].itemWindowHolder.SetActive(true);
            }
            else
            {
                itemStoreObjects[ctr].itemWindowHolder.SetActive(false);
            }
        }
    }

    public UIResizeScript armoryFrameAdjust;
    public DrawCameraOnSprite drawCamOnSprite;

    /*This function is used to disable the UIOverly and item scroll and purchse button
     to prevent these functions from conflicting with the process of random item or .*/
    public void activate_random_box_process()
    {

    }

    /*This function is used to restore regular functionality of armory when random item
     buying process is completed.*/
    public void disable_random_box_process()
    {

    }

    public StatChangeDisplay statChangeDisplay;

	// Use this for initialization
	void Start () {
        populate_armory_table();
        store_window_operator(0);
        statChangeDisplay.display_player_stats();
        armoryFrameAdjust.enabled = true;
        drawCamOnSprite.enabled = true;
	}
	
}
