using UnityEngine;
using System.Collections;

public class ItemGeneration: MonoBehaviour  {
    public int[] numHeadItemByTier;
    public int[] numBodyItemByTier;
    public int[] numWeaponItemByTier;

    public ItemGeneration() 
    {
    }

    /*tier is value from 1 - N*/
	public string gen_item (int tier) {
        string itemGen;
        string item_part1 = tier.ToString();
        while (item_part1.Length < 2)
        {
            item_part1 = item_part1.Insert(0, "0");
        }
        string item_part2 = "H";
        string item_part3 = "001";
        int num = Random.Range(0, 3);
        
        if (num == 0)
        {
            item_part2 = "H";
            int itemNum = Random.Range(1, numHeadItemByTier[tier - 1]);
            item_part3 = itemNum.ToString();
            while (item_part3.Length < 3)
            {
                item_part3 = item_part3.Insert(0, "0");
            }
        }
        else if (num == 1)
        {
            item_part2 = "W";
            int itemNum = Random.Range(1, numWeaponItemByTier[tier - 1]);
            item_part3 = itemNum.ToString();
            while (item_part3.Length < 3)
            {
                item_part3 = item_part3.Insert(0, "0");
            }
        }
        else if (num == 2)
        {
            item_part2 = "B";
            int itemNum = Random.Range(1, numHeadItemByTier[tier - 1]);
            item_part3 = itemNum.ToString();
            while (item_part3.Length < 3)
            {
                item_part3 = item_part3.Insert(0, "0");
                Debug.Log("B: " + item_part3.Length); 
            }
        }
        itemGen = item_part1 + item_part2 + item_part3;
        Debug.Log(itemGen);
        return itemGen;
	}	
}
