using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ItemInitializer : MonoBehaviour {
    public GameObject itemListPrefab;
    ItemDictionary prefabItemControlsScript;

    public GameObject itemPoolDataFormat;
    ItemPoolData prefabPoolScript;
    public float commonDropRatio;
    public float uncommonDropRatio;
    public float rareDropRatio;
    public float legendaryDropRatio;


    public int maxItemTier;

    string get_tier_folder_access(int tierInput)
    {
        string temp = "Items/Tier" + (tierInput + 1).ToString() + "/";
        Debug.Log(temp);
        return temp;
    }

    GameObject[] get_all_object_in_path(string dataPath)
    {
        Object[] getAssetRaw = Resources.LoadAll<Object>(dataPath);
        GameObject[] getAsset = new GameObject[getAssetRaw.Length];
        Debug.Log("Number of items loaded! " + getAssetRaw.Length);
        for (int ctr = 0; ctr < getAssetRaw.Length; ctr++)
        {
            getAsset[ctr] = (GameObject)getAssetRaw[ctr];
            Debug.Log("Item Names Loaded: " + getAsset[ctr].name);
        }
        return getAsset;
    }



	// Use this for initialization
	void Start () {
        prefabItemControlsScript = itemListPrefab.GetComponent<ItemDictionary>();
        prefabItemControlsScript.itemList = new ItemList[maxItemTier];
        prefabPoolScript = itemPoolDataFormat.GetComponent<ItemPoolData>();
        prefabPoolScript.itemData = new ItemTypeData[maxItemTier];

        for (int ctr = 0; ctr < maxItemTier; ctr++)
        {
            //string folderPath = get_tier_folder_access(ctr);
            GameObject[] itemTierList = get_all_object_in_path(get_tier_folder_access(ctr));
            prefabItemControlsScript.itemList[ctr].items = itemTierList;
            IList<GameObject> commonItemList = new List<GameObject>();
            IList<GameObject> uncommonItemList = new List<GameObject>();
            IList<GameObject> rareItemList = new List<GameObject>();
            IList<GameObject> legendaryItemList = new List<GameObject>();
            for (int itemOrganizer = 0; itemOrganizer < itemTierList.Length; itemOrganizer++)
            {
                Item tempItemAccess = itemTierList[itemOrganizer].GetComponent<Item>();

                if (tempItemAccess.itemRarity == Item.ItemRarity.COMMON)
                    commonItemList.Add(itemTierList[itemOrganizer]);

                else if (tempItemAccess.itemRarity == Item.ItemRarity.UNCOMMON)
                    uncommonItemList.Add(itemTierList[itemOrganizer]);

                else if (tempItemAccess.itemRarity == Item.ItemRarity.RARE)
                    rareItemList.Add(itemTierList[itemOrganizer]);

                else if (tempItemAccess.itemRarity == Item.ItemRarity.LEGENDARY)
                    legendaryItemList.Add(itemTierList[itemOrganizer]);

                
            }
            prefabPoolScript.itemData[ctr].itemTypePool = new ItemDB[4];
            prefabPoolScript.itemData[ctr].itemTypePool[0].type = "Common";
            prefabPoolScript.itemData[ctr].itemTypePool[0].dropRatio = commonDropRatio;
            prefabPoolScript.itemData[ctr].itemTypePool[0].itemsData = new GameObject[commonItemList.Count];
            commonItemList.CopyTo(prefabPoolScript.itemData[ctr].itemTypePool[0].itemsData, 0);

            prefabPoolScript.itemData[ctr].itemTypePool[1].type = "Uncommon";
            prefabPoolScript.itemData[ctr].itemTypePool[1].dropRatio = uncommonDropRatio;
            prefabPoolScript.itemData[ctr].itemTypePool[1].itemsData = new GameObject[uncommonItemList.Count];
            uncommonItemList.CopyTo(prefabPoolScript.itemData[ctr].itemTypePool[1].itemsData, 0);

            prefabPoolScript.itemData[ctr].itemTypePool[2].type = "Rare";
            prefabPoolScript.itemData[ctr].itemTypePool[2].dropRatio = rareDropRatio;
            prefabPoolScript.itemData[ctr].itemTypePool[2].itemsData = new GameObject[rareItemList.Count];
            rareItemList.CopyTo(prefabPoolScript.itemData[ctr].itemTypePool[2].itemsData, 0);

            prefabPoolScript.itemData[ctr].itemTypePool[3].type = "Legendary";
            prefabPoolScript.itemData[ctr].itemTypePool[3].dropRatio = legendaryDropRatio;
            prefabPoolScript.itemData[ctr].itemTypePool[3].itemsData = new GameObject[legendaryItemList.Count];
            legendaryItemList.CopyTo(prefabPoolScript.itemData[ctr].itemTypePool[3].itemsData, 0);
        }
	}
}
