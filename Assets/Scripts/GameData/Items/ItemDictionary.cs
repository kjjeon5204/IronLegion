using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDictionary : MonoBehaviour {
    public ItemList[] itemList;
    IDictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

    public GameObject get_item_data(string itemID)
    {
        if (itemDictionary.ContainsKey(itemID))
            return itemDictionary[itemID];
        else
            return null;
    }

	// Use this for initialization
	void Start () {
        for (int tierCtr = 0; tierCtr < itemList.Length; tierCtr++)
        {
            for (int itemAccess = 0; itemAccess < itemList[tierCtr].items.Length; 
                itemAccess++)
            {
                itemDictionary[itemList[tierCtr].items[itemAccess].name] =
                    itemList[tierCtr].items[itemAccess];
            }
        }
	}
}
