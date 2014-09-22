using UnityEngine;
using System.Collections;

public class PlayerStatComparison : MonoBehaviour {
    bool initialized = false;
    public ItemDictionary itemDictionary;
    Item[] itemList;

    int modifiedAttack;
    int modifiedArmor;
    int modifiedHealth;
    int modifiedEnergy;
    int modifiedluck;
    int modifiedPenetration;


    public TextMesh[] statChangeDisplay;
    public TextMesh[] itemStatDisplay;

    void initialize_table() 
    {
        HeroStats currentPlayerData = new HeroStats();
        currentPlayerData.load_data();
        string[] equippedItemStringList = currentPlayerData.get_equipped_item();
        itemList = new Item[equippedItemStringList.Length];
        for (int ctr = 0; ctr < equippedItemStringList.Length; ctr++)
        {
            if (equippedItemStringList[ctr] != "000000")
                itemList[ctr] = itemDictionary.get_item_data(equippedItemStringList[ctr]).GetComponent<Item>();
        }
        initialized = true;
    }

    public void get_modified_stat(Item itemEquipped, Item itemBuying)
    {
        if (itemEquipped != null)
        {
            modifiedAttack = (int)(itemBuying.damage - itemEquipped.damage);
            modifiedArmor = (int)(itemBuying.armor - itemEquipped.armor);
            modifiedHealth = (int)(itemBuying.hp - itemEquipped.hp);
            modifiedEnergy = (int)(itemBuying.energy - itemEquipped.energy);
            modifiedluck = (int)(itemBuying.luck - itemEquipped.luck);
            modifiedPenetration = (int)(itemBuying.penetration - itemEquipped.penetration);
        }
        else
        {
            modifiedAttack = (int)itemBuying.damage;
            modifiedArmor = (int)itemBuying.armor;
            modifiedHealth = (int)itemBuying.hp;
            modifiedEnergy = (int)itemBuying.energy;
            modifiedluck = (int)itemBuying.luck;
            modifiedPenetration = (int)itemBuying.penetration;
        }
    }

    public void compare_player_stat(Item inputItem)
    {
        if (initialized == false)
        {
            initialize_table();
        }
        if (inputItem.itemType == Item.ItemType.HEAD)
        {
            get_modified_stat(itemList[0], inputItem);
        }
        else if (inputItem.itemType == Item.ItemType.WEAPON)
        {
            get_modified_stat(itemList[1], inputItem);
        }
        else if (inputItem.itemType == Item.ItemType.ARMOR)
        {
            get_modified_stat(itemList[2], inputItem);
        }
        else if (inputItem.itemType == Item.ItemType.CORE)
        {
            if (itemList[3] != null && itemList[4] != null && 
                itemList[3].energy > itemList[4].energy)
            {
                get_modified_stat(itemList[3], inputItem);
            }
            else
            {
                Debug.Log("Check player data");
                get_modified_stat(itemList[4], inputItem);
            }
        }



        int textBoxStatTracker = 0;
        for (int ctr = 0; ctr < itemStatDisplay.Length; ctr++)
        {
            itemStatDisplay[ctr].gameObject.SetActive(false);
        }

        if (inputItem.damage != 0)
        {
            itemStatDisplay[textBoxStatTracker].gameObject.SetActive(true);
            itemStatDisplay[textBoxStatTracker].text = "ATK " + inputItem.damage.ToString();
            textBoxStatTracker++;
        }
        if (inputItem.armor != 0)
        {
            itemStatDisplay[textBoxStatTracker].gameObject.SetActive(true);
            itemStatDisplay[textBoxStatTracker].text = "DEF " + inputItem.armor.ToString();
            textBoxStatTracker++;
        }
        if (inputItem.hp != 0)
        {
            itemStatDisplay[textBoxStatTracker].gameObject.SetActive(true);
            itemStatDisplay[textBoxStatTracker].text = "HP  " + inputItem.hp.ToString();
            textBoxStatTracker++;
        }
        if (inputItem.energy != 0)
        {
            itemStatDisplay[textBoxStatTracker].gameObject.SetActive(true);
            itemStatDisplay[textBoxStatTracker].text = "EN  " + inputItem.energy.ToString();
            textBoxStatTracker++;
        }
        if (inputItem.luck != 0)
        {
            itemStatDisplay[textBoxStatTracker].gameObject.SetActive(true);
            itemStatDisplay[textBoxStatTracker].text = "LUK " + inputItem.luck.ToString();
            textBoxStatTracker++;
        }
        if (inputItem.penetration != 0)
        {
            itemStatDisplay[textBoxStatTracker].gameObject.SetActive(true);
            itemStatDisplay[textBoxStatTracker].text = "PEN " + inputItem.penetration.ToString();
            textBoxStatTracker++;
        }

        for (int ctr = 0; ctr < statChangeDisplay.Length; ctr++)
        {
            statChangeDisplay[ctr].gameObject.SetActive(false);
        }

        textBoxStatTracker = 0;
        if (modifiedAttack != 0)
        {
            statChangeDisplay[textBoxStatTracker].gameObject.SetActive(true);
            statChangeDisplay[textBoxStatTracker].text = "ATK " + modifiedAttack.ToString();
            textBoxStatTracker++;
        }
        if (modifiedArmor != 0)
        {
            statChangeDisplay[textBoxStatTracker].gameObject.SetActive(true);
            statChangeDisplay[textBoxStatTracker].text = "DEF" + modifiedArmor.ToString();
            textBoxStatTracker++;
        }
        if (modifiedHealth != 0)
        {
            statChangeDisplay[textBoxStatTracker].gameObject.SetActive(true);
            statChangeDisplay[textBoxStatTracker].text = "HP  " + modifiedHealth.ToString();
            textBoxStatTracker++;
        }
        if (modifiedEnergy != 0)
        {
            statChangeDisplay[textBoxStatTracker].gameObject.SetActive(true);
            statChangeDisplay[textBoxStatTracker].text = "EN  " + modifiedEnergy.ToString();
            textBoxStatTracker++;
        }
        if (modifiedluck != 0)
        {
            statChangeDisplay[textBoxStatTracker].gameObject.SetActive(true);
            statChangeDisplay[textBoxStatTracker].text = "LUK  " + modifiedluck.ToString();
            textBoxStatTracker++;
        }
        if (modifiedPenetration != 0)
        {
            statChangeDisplay[textBoxStatTracker].gameObject.SetActive(true);
            statChangeDisplay[textBoxStatTracker].text = "PEN " + modifiedPenetration.ToString();
            textBoxStatTracker++;
        }
    }
}
