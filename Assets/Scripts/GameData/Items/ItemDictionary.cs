using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemDictionary : MonoBehaviour {
    public ItemList[] itemList;


    IList<GameObject> headItemList;
    IList<GameObject> coreItemList;
    IList<GameObject> bodyItemList;
    IList<GameObject> weaponItemList;

    struct ItemTableAccess
    {
        public int itemTier;
        public int itemNum;
    }

    IDictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();
    bool setPoolTier = false;

    IDictionary<string, ItemTableAccess> itemIDDictionary = new Dictionary<string, ItemTableAccess>();
    IDictionary<string, ItemTableAccess> itemNameDictionary = new Dictionary<string, ItemTableAccess>();


    public GameObject get_item_data(string itemID)
    {
        if (itemIDDictionary.ContainsKey(itemID))
        {
            ItemTableAccess acc = itemIDDictionary[itemID];
            return itemList[acc.itemTier].items[acc.itemNum];
        }
        else
            return null;
    }

    public void set_pooling_tier(int itemTier)
    {
        if (setPoolTier == false)
        {
            int minTier = itemTier - 2;
            if (minTier < 0)
            {
                minTier = 0;
            }

            for (int ctr = minTier; ctr <= itemTier; ctr++)
            {
                headItemList = new List<GameObject>();
                coreItemList = new List<GameObject>();
                bodyItemList = new List<GameObject>();
                weaponItemList = new List<GameObject>();
                for (int ctr2 = 0; ctr2 < itemList[ctr].items.Length; ctr2++)
                {
                    Item temp = itemList[ctr].items[ctr2].GetComponent<Item>();
                    if (temp.itemType == Item.ItemType.ARMOR)
                        bodyItemList.Add(temp.gameObject);
                    else if (temp.itemType == Item.ItemType.CORE)
                        coreItemList.Add(temp.gameObject);
                    else if (temp.itemType == Item.ItemType.HEAD)
                        headItemList.Add(temp.gameObject);
                    else if (temp.itemType == Item.ItemType.WEAPON)
                        weaponItemList.Add(temp.gameObject);
                }
            }
            //setPoolTier = true;
        }
    }

    public GameObject generate_random_item(int minTier, int maxTier)
    {
        int tierAccess = Random.Range(minTier, maxTier);
        int itemAccess = Random.Range(0, itemList[tierAccess].items.Length - 1);
        return itemList[tierAccess].items[itemAccess];
    }


    public GameObject generate_random_item(int itemTier)
    {
        int numberOfItems = itemList[itemTier].items.Length;
        int itemAccess = Random.Range(0, numberOfItems);
        return itemList[itemTier].items[itemAccess];
    }


    public GameObject generate_random_item(Item.ItemType itemType)
    {
        if (itemType == Item.ItemType.ARMOR)
        {
            int itemGen = Random.Range(0, bodyItemList.Count);
            return bodyItemList[itemGen];
        }
        else if (itemType == Item.ItemType.CORE)
        {
            int itemGen = Random.Range(0, coreItemList.Count);
            return coreItemList[itemGen];
        }
        else if (itemType == Item.ItemType.HEAD)
        {
            int itemGen = Random.Range(0, headItemList.Count);
            return headItemList[itemGen];
        }
        else 
        {
            int itemGen = Random.Range(0, weaponItemList.Count);
            return weaponItemList[itemGen];
        }
    }

	// Use this for initialization
	void Start () {
        Debug.Log("Dictionary initialized!");
        for (int tierCtr = 0; tierCtr < itemList.Length; tierCtr++)
        {
            for (int itemAccess = 0; itemAccess < itemList[tierCtr].items.Length; 
                itemAccess++)
            {
                ItemTableAccess tempItemAccess;
                tempItemAccess.itemTier = tierCtr;
                tempItemAccess.itemNum = itemAccess;

                Item tempItem = itemList[tierCtr].items[itemAccess].GetComponent<Item>();
                itemIDDictionary[tempItem.gameObject.name] = tempItemAccess;
                itemNameDictionary[tempItem.itemName] = tempItemAccess;
            }
        }
	}
}
