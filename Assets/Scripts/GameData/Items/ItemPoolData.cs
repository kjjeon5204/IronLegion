using UnityEngine;
using System.Collections;

//used for tier
[System.Serializable]
public struct ItemTypeData {
    public string Tier;
    public ItemDB[] itemTypePool;
}

//used for type
[System.Serializable]
public struct ItemDB
{
    public string type;
	public float dropRatio;
    public GameObject[] itemsData;
}

[System.Serializable]
public struct IndividualItem
{
    public GameObject itemData;
}


public class ItemPoolData : MonoBehaviour {
    public ItemTypeData[] itemData;


    public GameObject get_item_table_modified_rate(float common, float uncommon,
        float rare, float legendary, int tier)
    {
        int tierAcc = tier - 1;

        int commonPoll = (int)(1000.0f * common);
        int uncommonPoll = commonPoll + (int)(1000.0f * uncommon);
        int rarePoll = uncommonPoll + (int)(1000.0f * rare);
        int legendaryPoll = rarePoll + (int)(1000.0f * legendary);

        int curPoll = Random.Range(0 * 1000, 100001);

        if (curPoll < commonPoll)
        {//common
            if (itemData[tierAcc].itemTypePool[0].itemsData.Length == 0)
                return null;
            int itemPoll = Random.Range(0, itemData[tierAcc].itemTypePool[0].itemsData.Length);
            return itemData[tierAcc].itemTypePool[0].itemsData[itemPoll];
        }
        else if (curPoll < uncommonPoll)
        {//uncommon
            if (itemData[tierAcc].itemTypePool[1].itemsData.Length == 0)
                return null;
            int itemPoll = Random.Range(0, itemData[tierAcc].itemTypePool[1].itemsData.Length);
            return itemData[tierAcc].itemTypePool[1].itemsData[itemPoll];
        }
        else if (curPoll < rarePoll)
        {//rare
            if (itemData[tierAcc].itemTypePool[2].itemsData.Length == 0)
                return null;
            int itemPoll = Random.Range(0, itemData[tierAcc].itemTypePool[2].itemsData.Length);
            return itemData[tierAcc].itemTypePool[2].itemsData[itemPoll];
        }
        else if (curPoll < legendaryPoll)
        {//legendary
            if (itemData[tierAcc].itemTypePool[3].itemsData.Length == 0)
                return null;
            int itemPoll = Random.Range(0, itemData[tierAcc].itemTypePool[3].itemsData.Length);
            return itemData[tierAcc].itemTypePool[3].itemsData[itemPoll];
        }
        else
        {
            return null;
        }
    }

	public GameObject get_item_table(int luck, int tier) {
		int tierAcc = tier - 1;

		int commonPoll = (int)(1000.0f * itemData[tierAcc].itemTypePool[0].dropRatio);
		int uncommonPoll = commonPoll + (int)(1000.0f * itemData[tierAcc].itemTypePool[1].dropRatio);
		int rarePoll = uncommonPoll + (int)(1000.0f * itemData[tierAcc].itemTypePool[2].dropRatio);
		int legendaryPoll = rarePoll + (int)(1000.0f * itemData[tierAcc].itemTypePool[3].dropRatio);

		int curPoll = Random.Range (0 + luck * 1000, 100001);

		if (curPoll < commonPoll) {//common
			if (itemData[tierAcc].itemTypePool[0].itemsData.Length == 0) 
				return null;
			int itemPoll = Random.Range (0, itemData[tierAcc].itemTypePool[0].itemsData.Length);
			return itemData[tierAcc].itemTypePool[0].itemsData[itemPoll];
		}
		else if (curPoll < uncommonPoll) {//uncommon
			if (itemData[tierAcc].itemTypePool[1].itemsData.Length == 0) 
				return null;
			int itemPoll = Random.Range (0, itemData[tierAcc].itemTypePool[1].itemsData.Length);
			return itemData[tierAcc].itemTypePool[1].itemsData[itemPoll];
		}
		else if (curPoll < rarePoll) {//rare
			if (itemData[tierAcc].itemTypePool[2].itemsData.Length == 0) 
				return null;
			int itemPoll = Random.Range (0, itemData[tierAcc].itemTypePool[2].itemsData.Length);
			return itemData[tierAcc].itemTypePool[2].itemsData[itemPoll];
		}
		else if ( curPoll < legendaryPoll) {//legendary
			if (itemData[tierAcc].itemTypePool[3].itemsData.Length == 0) 
				return null;
			int itemPoll = Random.Range (0, itemData[tierAcc].itemTypePool[3].itemsData.Length);
			return itemData[tierAcc].itemTypePool[3].itemsData[itemPoll];
		}
		else {
			return null;
		}
	}
}



