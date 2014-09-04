using UnityEngine;
using System.Collections;

public class ItemEquipOption : MonoBehaviour {
    Item genItem;
    ItemDictionary itemDictionary;


    public void set_generated_item(Item generatedItem)
    {
        genItem = generatedItem;
    }

    void Clicked()
    {
        string currentlyEquipped = "000000";
        HeroStats curHeroStat = new HeroStats();
        Inventory playerInventory = new Inventory();
        playerInventory.load_inventory();
        curHeroStat.load_data();
        if (genItem.itemID[2] == 'H')
        {
            currentlyEquipped = curHeroStat.get_equipped_item(0);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                curHeroStat.remove_item(curEquippedItem);
                playerInventory.add_item(currentlyEquipped);
                curHeroStat.equip_item(genItem);
            }
        }
        else if (genItem.itemID[2] == 'C')
        {
            currentlyEquipped = curHeroStat.get_equipped_item(3);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                curHeroStat.remove_item(curEquippedItem);
                playerInventory.add_item(currentlyEquipped);
                curHeroStat.equip_item(genItem);
            }
        }
        else if (genItem.itemID[2] == 'B')
        {
            currentlyEquipped = curHeroStat.get_equipped_item(2);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                curHeroStat.remove_item(curEquippedItem);
                playerInventory.add_item(currentlyEquipped);
                curHeroStat.equip_item(genItem);
            }
        }
        else if (genItem.itemID[2] == 'W')
        {
            currentlyEquipped = curHeroStat.get_equipped_item(1);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                curHeroStat.remove_item(curEquippedItem);
                playerInventory.add_item(currentlyEquipped);
                curHeroStat.equip_item(genItem);
            }
        }
        playerInventory.store_inventory();
        curHeroStat.save_data();
    }
}
