using UnityEngine;
using System.Collections;

public class ArmoryControl : MonoBehaviour {
    public enum CatalogType
    {
        HEAD,
        BODY,
        WEAPON,
        CORE,
        UPGRADE
    }

    public ArmoryDataControl armoryData;
    public ItemDisplayWindow headCatalog;
    public ItemDisplayWindow bodyCatalog;
    public ItemDisplayWindow weaponCatalog;
    public ItemDisplayWindow coreCatalog;
    public UpgradeWindow upgradeCatalog;

    public GameObject armorButton;
    public GameObject headButton;
    public GameObject bodyButton;
    public GameObject weaponButton;
    public GameObject upgradeButton;
    

    public Camera armoryItemSlotCam;
    public SpriteRenderer armoryItemFrame;
    Rect spriteRectViewport;


    public TextMesh cogentumOwnedDisplay;
    public TextMesh creditOwnedDisplay;

    int creditOwned;
    int cogentumOwned;
    int totalUnlockedSlotCount;
    int totalUnlockedSpot;

    public PlayerStatComparison statComparison;
    public GameObject selectedItem;

    public Camera itemSlotUICam;

    public ItemControls inventoryAccess;


    public int get_owned_cogentum()
    {
        return cogentumOwned;
    }

    public int get_owned_credit()
    {
        return creditOwned;
    }

    public void set_selectedItem(GameObject itemSelected)
    {
        selectedItem = itemSelected;
        statComparison.compare_player_stat(selectedItem.GetComponent<Item>());
    }

    public void disable_all_frame()
    {
        headCatalog.gameObject.SetActive(false);
        bodyCatalog.gameObject.SetActive(false);
        weaponCatalog.gameObject.SetActive(false);
        coreCatalog.gameObject.SetActive(false);
        upgradeCatalog.gameObject.SetActive(false);
    }

    public void item_bought(int creditSpent, int cogentumSpent, int slotNum, Item.ItemType itemType)
    {
        creditOwned -= creditSpent;
        cogentumOwned -= cogentumSpent;
        creditOwnedDisplay.text = creditOwned.ToString();
        cogentumOwnedDisplay.text = cogentumOwned.ToString();

        if (itemType == Item.ItemType.HEAD)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned, slotNum);
            bodyCatalog.currency_update(creditOwned, cogentumOwned);
            weaponCatalog.currency_update(creditOwned, cogentumOwned);
            coreCatalog.currency_update(creditOwned, cogentumOwned);
        }
        else if (itemType == Item.ItemType.ARMOR)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned);
            bodyCatalog.currency_update(creditOwned, cogentumOwned, slotNum);
            weaponCatalog.currency_update(creditOwned, cogentumOwned);
            coreCatalog.currency_update(creditOwned, cogentumOwned);
        }
        else if (itemType == Item.ItemType.WEAPON)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned);
            bodyCatalog.currency_update(creditOwned, cogentumOwned);
            weaponCatalog.currency_update(creditOwned, cogentumOwned, slotNum);
            coreCatalog.currency_update(creditOwned, cogentumOwned);
        }
        else if (itemType == Item.ItemType.CORE)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned);
            bodyCatalog.currency_update(creditOwned, cogentumOwned);
            weaponCatalog.currency_update(creditOwned, cogentumOwned);
            coreCatalog.currency_update(creditOwned, cogentumOwned, slotNum);
        }
    }

    public void unlock_slot(Item.ItemType itemType, int creditSpent, int cogentumSpent) {
        creditOwned -= creditSpent;
        cogentumOwned -= cogentumSpent;
        creditOwnedDisplay.text = creditOwned.ToString();
        cogentumOwnedDisplay.text = cogentumOwned.ToString();

        if (itemType == Item.ItemType.HEAD)
        {
            headCatalog.unlock_slot(creditOwned, cogentumOwned);
            bodyCatalog.currency_update(creditOwned, cogentumOwned);
            weaponCatalog.currency_update(creditOwned, cogentumOwned);
            coreCatalog.currency_update(creditOwned, cogentumOwned);
        }
        else if (itemType == Item.ItemType.ARMOR)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned);
            bodyCatalog.unlock_slot(creditOwned, cogentumOwned);
            weaponCatalog.currency_update(creditOwned, cogentumOwned);
            coreCatalog.currency_update(creditOwned, cogentumOwned);
        }
        else if (itemType == Item.ItemType.WEAPON)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned);
            bodyCatalog.currency_update(creditOwned, cogentumOwned);
            weaponCatalog.unlock_slot(creditOwned, cogentumOwned);
            coreCatalog.currency_update(creditOwned, cogentumOwned);
        }
        else if (itemType == Item.ItemType.CORE)
        {
            headCatalog.currency_update(creditOwned, cogentumOwned);
            bodyCatalog.currency_update(creditOwned, cogentumOwned);
            weaponCatalog.currency_update(creditOwned, cogentumOwned);
            coreCatalog.unlock_slot(creditOwned, cogentumOwned);
        }
    }


    void catalog_controls(CatalogType curCatalog)
    {
        if (curCatalog == CatalogType.HEAD)
        {
            headCatalog.gameObject.SetActive(true);
            bodyCatalog.gameObject.SetActive(false);
            weaponCatalog.gameObject.SetActive(false);
            coreCatalog.gameObject.SetActive(false);
            upgradeCatalog.gameObject.SetActive(false);
        }
        else if (curCatalog == CatalogType.BODY)
        {
            headCatalog.gameObject.SetActive(false);
            bodyCatalog.gameObject.SetActive(true);
            weaponCatalog.gameObject.SetActive(false);
            coreCatalog.gameObject.SetActive(false);
            upgradeCatalog.gameObject.SetActive(false);
        }
        else if (curCatalog == CatalogType.WEAPON)
        {
            headCatalog.gameObject.SetActive(false);
            bodyCatalog.gameObject.SetActive(false);
            weaponCatalog.gameObject.SetActive(true);
            coreCatalog.gameObject.SetActive(false);
            upgradeCatalog.gameObject.SetActive(false);
        }
        else if (curCatalog == CatalogType.CORE)
        {
            headCatalog.gameObject.SetActive(false);
            bodyCatalog.gameObject.SetActive(false);
            weaponCatalog.gameObject.SetActive(false);
            coreCatalog.gameObject.SetActive(true);
            upgradeCatalog.gameObject.SetActive(false);
        }
        else if (curCatalog == CatalogType.UPGRADE)
        {
            headCatalog.gameObject.SetActive(false);
            bodyCatalog.gameObject.SetActive(false);
            weaponCatalog.gameObject.SetActive(false);
            coreCatalog.gameObject.SetActive(false);
            upgradeCatalog.gameObject.SetActive(true);
        }
    }


    void OnEnable()
    {
        float offSet = 0.2f;
        Vector3 worldPosition;
        spriteRectViewport = new Rect();
        worldPosition = armoryItemFrame.bounds.center;
        worldPosition.x -= armoryItemFrame.bounds.extents.x - offSet;
        spriteRectViewport.x = Camera.main.WorldToViewportPoint(worldPosition).x;

        worldPosition = armoryItemFrame.bounds.center;
        worldPosition.x += armoryItemFrame.bounds.extents.x - offSet;
        spriteRectViewport.width = Camera.main.WorldToViewportPoint(worldPosition).x -
            spriteRectViewport.x;

        worldPosition = armoryItemFrame.bounds.center;
        worldPosition.y -= armoryItemFrame.bounds.extents.y - offSet;
        spriteRectViewport.y = Camera.main.WorldToViewportPoint(worldPosition).y;

        worldPosition = armoryItemFrame.bounds.center;
        worldPosition.y += armoryItemFrame.bounds.extents.y - offSet;
        spriteRectViewport.height = Camera.main.WorldToViewportPoint(worldPosition).y -
            spriteRectViewport.y;

        armoryItemSlotCam.rect = spriteRectViewport;

        catalog_controls(CatalogType.CORE);
    }

    void update_total_unlocked_slot()
    {
        totalUnlockedSlotCount = headCatalog.get_unlocked_slot_count();
        totalUnlockedSlotCount += bodyCatalog.get_unlocked_slot_count();
        totalUnlockedSlotCount += weaponCatalog.get_unlocked_slot_count();
        totalUnlockedSlotCount += coreCatalog.get_unlocked_slot_count();
    }

    public void reset_catalog_data()
    {
        StoreData tempStoreData = armoryData.generate_new_store_data(Item.ItemType.HEAD);
        headCatalog.reinitialize_store_data(tempStoreData);

        tempStoreData = armoryData.generate_new_store_data(Item.ItemType.ARMOR);
        bodyCatalog.reinitialize_store_data(tempStoreData);

        tempStoreData = armoryData.generate_new_store_data(Item.ItemType.WEAPON);
        weaponCatalog.reinitialize_store_data(tempStoreData);

        tempStoreData = armoryData.generate_new_store_data(Item.ItemType.CORE);
        coreCatalog.reinitialize_store_data(tempStoreData);
    }

	// Use this for initialization
	void Start () {
        Inventory playerInventory = new Inventory();
        playerInventory.load_inventory();
        creditOwned = playerInventory.get_currency();
        creditOwnedDisplay.text = creditOwned.ToString();

        cogentumOwned = playerInventory.get_paid_currency();
        cogentumOwnedDisplay.text = cogentumOwned.ToString();
        catalog_controls(CatalogType.BODY);

        StoreData tempStoreData = armoryData.head_catalog_data();
        totalUnlockedSlotCount += tempStoreData.numberOfUnlockedSpot;
        headCatalog.initialize_store_data(tempStoreData, this);

        tempStoreData = armoryData.main_frame_catalog_data();
        totalUnlockedSlotCount += tempStoreData.numberOfUnlockedSpot;
        bodyCatalog.initialize_store_data(tempStoreData, this);

        tempStoreData = armoryData.weapon_catalog_data();
        totalUnlockedSlotCount += tempStoreData.numberOfUnlockedSpot;
        weaponCatalog.initialize_store_data(tempStoreData, this);

        tempStoreData = armoryData.core_catalog_data();
        totalUnlockedSlotCount += tempStoreData.numberOfUnlockedSpot;
        coreCatalog.initialize_store_data(tempStoreData, this);
	}
}
