using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public struct ArmoryCatalog
{
    public string itemID;
    public GameObject itemObject;
    public int itemSlotPosition;
    public bool itemSaleStatus;
}

public struct StoreData
{
    public Item.ItemType catalogType;
    public int numberOfUnlockedSpot;
    public IList<ArmoryCatalog> soldItemList;
}



public class ArmoryDataControl : MonoBehaviour
{
    public ItemDictionary itemDictionary;
    int playerLevel = 6;

    System.DateTime curTime;
    string readCurTime;
    string nextResetTime;
    public ItemPoolData itemPooling;
    public HeroLevelData heroLevelData;



    ArmoryCatalog initialize_item(GameObject itemObject, int itemSlotPosition,
        bool itemSaleStatus)
    {
        ArmoryCatalog tempCatalogData = new ArmoryCatalog();
        Item readItem = itemObject.GetComponent<Item>();
        tempCatalogData.itemID = readItem.itemID;
        tempCatalogData.itemObject = itemObject;
        tempCatalogData.itemSaleStatus = itemSaleStatus;
        tempCatalogData.itemSlotPosition = itemSlotPosition;

        return tempCatalogData;
    }



    public StoreData head_catalog_data() {
        return read_catalog_data("/StoreData/HeadCatalog.txt", Item.ItemType.HEAD);
    }

    public StoreData main_frame_catalog_data() {
        return read_catalog_data("/StoreData/BodyCatalog.txt", Item.ItemType.ARMOR);
    }

    public StoreData weapon_catalog_data() {
        return read_catalog_data("/StoreData/WeaponCatalog.txt", Item.ItemType.WEAPON);
    }

    public StoreData core_catalog_data() {
        return read_catalog_data("/StoreData/CoreCatalog.txt", Item.ItemType.CORE);
    }


    StoreData read_catalog_data(string fileName, Item.ItemType catalogType)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/StoreData"))
            Directory.CreateDirectory(Application.persistentDataPath + "/StoreData");
        string dataPath = Application.persistentDataPath + fileName;
        StoreData temp = new StoreData();
        temp.soldItemList = new List<ArmoryCatalog>();
        temp.catalogType = catalogType;
        if (File.Exists(dataPath))
        {
            using (StreamReader inputFile = File.OpenText(dataPath))
            {
                temp.numberOfUnlockedSpot = System.Convert.ToInt32(inputFile.ReadLine());
                for (int ctr = 0; ctr < 3; ctr++)
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

                    temp.catalogType = catalogType;
                    temp.soldItemList.Add(tempItem);
                }
            }
            return temp;
        }
        else
        {
            temp.numberOfUnlockedSpot = 1;
            temp.catalogType = catalogType;
            temp.soldItemList = new List<ArmoryCatalog>();
            for (int ctr = 0; ctr < 3; ctr++)
            {
                ArmoryCatalog tempItem = new ArmoryCatalog();
                playerLevel = heroLevelData.get_player_level();
                int itemPoolNum = Random.Range(playerLevel - 2, playerLevel + 1);
                Debug.Log(itemPoolNum);
                itemDictionary.set_pooling_tier(itemPoolNum);
                tempItem = initialize_item(itemDictionary.
                    generate_random_item (catalogType), ctr, false);
                temp.soldItemList.Add(tempItem);
            }
            return temp;
        }
    }

    public StoreData generate_new_store_data(Item.ItemType catalogTypeIn)
    {
        StoreData temp = new StoreData();
        temp.numberOfUnlockedSpot = 1;
        temp.soldItemList = new List<ArmoryCatalog>();

        temp.catalogType = catalogTypeIn;
        for (int ctr = 0; ctr < 3; ctr++)
        {
            ArmoryCatalog tempItem = new ArmoryCatalog();
            playerLevel = heroLevelData.get_player_level();
            int itemPoolNum = Random.Range(playerLevel - 2, playerLevel + 1);
            Debug.Log(itemPoolNum);
            itemDictionary.set_pooling_tier(itemPoolNum);
            tempItem = initialize_item(itemDictionary.
                generate_random_item(catalogTypeIn), ctr, false);
            temp.soldItemList.Add(tempItem);
        }
        return temp;
    }

    public void save_store_data(StoreData myInventory)
    {
        if (myInventory.catalogType == Item.ItemType.ARMOR)
        {
            save_store_data("/StoreData/BodyCatalog.txt", myInventory);
        }
        else if (myInventory.catalogType == Item.ItemType.CORE)
        {
            save_store_data("/StoreData/CoreCatalog.txt", myInventory);
        }
        else if (myInventory.catalogType == Item.ItemType.HEAD)
        {
            save_store_data("/StoreData/HeadCatalog.txt", myInventory);
        }
        else if (myInventory.catalogType == Item.ItemType.WEAPON)
        {
            save_store_data("/StoreData/WeaponCatalog.txt", myInventory);
        }
    }

    void save_store_data(string fileName, StoreData itemStore)
    {
        string dataPath = Application.persistentDataPath + fileName;
        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine(itemStore.numberOfUnlockedSpot);
            for (int ctr = 0; ctr < 3; ctr++)
            {
                outFile.WriteLine(itemStore.soldItemList[ctr].itemID);
                outFile.WriteLine(itemStore.soldItemList[ctr].itemSlotPosition);
                if (itemStore.soldItemList[ctr].itemSaleStatus == true)
                {
                    outFile.WriteLine("SOLD");
                }
                if (itemStore.soldItemList[ctr].itemSaleStatus == false)
                {
                    outFile.WriteLine("NOT_SOLD");
                }
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
