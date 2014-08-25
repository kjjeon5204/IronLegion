using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public struct ArmoryCatalog
{
    public string itemID;
    public GameObject itemObject;
    public int creditRequirement;
    public int cogentumRequirement;
    public int itemSlotPosition;
    public bool itemSaleStatus;
}

public struct StoreData
{
    public Item.ItemType catalogType;
    public IList<ArmoryCatalog> soldItemList;
}



public class ArmoryDataControl : MonoBehaviour
{
    public ItemDictionary itemDictionary;

    System.DateTime curTime;
    string readCurTime;
    string nextResetTime;
    ItemPoolData itemPooling;

    StoreData headCatalog;
    StoreData mainFrameCatalog;
    StoreData weaponCatalog;
    StoreData coreCatalog;



    ArmoryCatalog initialize_item(GameObject itemObject, int itemSlotPosition,
        bool itemSaleStatus)
    {
        ArmoryCatalog tempCatalogData = new ArmoryCatalog();
        Item readItem = itemObject.GetComponent<Item>();
        tempCatalogData.cogentumRequirement = readItem.cg_price;
        tempCatalogData.creditRequirement = readItem.buy_price;
        tempCatalogData.itemID = readItem.itemID;
        tempCatalogData.itemObject = itemObject;
        tempCatalogData.itemSlotPosition = itemSlotPosition;

        return tempCatalogData;
    }


    public StoreData head_catalog_data() {
        return headCatalog;
    }

    public StoreData main_frame_catalog_data() {
        return mainFrameCatalog;
    }

    public StoreData weapon_catalog_data() {
        return weaponCatalog;
    }

    public StoreData core_catalog_data()
    {
        return coreCatalog;
    }


    StoreData read_catalog_data(string fileName, Item.ItemType catalogType)
    {
        string dataPath = Application.persistentDataPath + fileName;
        StoreData temp = new StoreData();
        using (StreamReader inputFile = File.OpenText(dataPath))
        {
            for (int ctr = 0; ctr < 5; ctr++)
            {
                ArmoryCatalog tempItem = new ArmoryCatalog();
                string itemName = inputFile.ReadLine();
                int itemSlotPos = System.Convert.ToInt32(inputFile.ReadLine());
                string saleStatusRaw = inputFile.ReadLine();
                bool saleStatus = false;
                if (saleStatusRaw == "NOT_SOLD")
                    saleStatus = false;
                else if (saleStatusRaw == "SOLD")
                    saleStatus = true;


                tempItem = initialize_item(itemDictionary.get_item_data(itemName), 
                    itemSlotPos, saleStatus);

                /*
                Item.ItemType itemType = tempItem.itemObject.GetComponent<Item>().itemType;
                if (itemType == Item.ItemType.ARMOR)
                    mainFrameCatalog.soldItemList.Add(tempItem);
                else if (itemType == Item.ItemType.CORE)
                    coreCatalog.soldItemList.Add(tempItem);
                else if (itemType == Item.ItemType.HEAD)
                    headCatalog.soldItemList.Add(tempItem);
                else if (itemType == Item.ItemType.WEAPON)
                    weaponCatalog.soldItemList.Add(tempItem);
                 */
                temp.catalogType = catalogType;
                temp.soldItemList.Add(tempItem);
            }
        }
        return temp;
    }

    public void save_store_data(Item.ItemType catalogType, StoreData myInventory)
    {
        if (catalogType == Item.ItemType.ARMOR)
        {
            save_store_data("ArmorCatalog.txt", myInventory);
        }
        else if (catalogType == Item.ItemType.CORE)
        {
            save_store_data("CoreCatalog.txt", myInventory);
        }
        else if (catalogType == Item.ItemType.HEAD)
        {
            save_store_data("HeadCatalog.txt", myInventory);
        }
        else if (catalogType == Item.ItemType.WEAPON)
        {
            save_store_data("WeaponCatalog.txt", myInventory);
        }
    }

    void save_store_data(string fileName, StoreData itemStore)
    {
        string dataPath = Application.persistentDataPath + fileName;
        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            for (int ctr = 0; ctr < 5; ctr++)
            {
                outFile.WriteLine(itemStore.soldItemList[ctr].itemID);
                outFile.WriteLine(itemStore.soldItemList[ctr].itemSlotPosition);
                if (itemStore.soldItemList[ctr].itemSaleStatus == true)
                    outFile.WriteLine("SOLD");
                if (itemStore.soldItemList[ctr].itemSaleStatus == false)
                    outFile.WriteLine("NOT_SOLD");
            }
        }
    }

    

    // Use this for initialization
    void Start()
    {

        curTime = System.DateTime.UtcNow;
    }

    // Update is called once per frame
    void Update()
    {
        readCurTime = curTime.ToString();
    }
}
