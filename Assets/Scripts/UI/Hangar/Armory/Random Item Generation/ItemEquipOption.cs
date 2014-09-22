using UnityEngine;
using System.Collections;

public class ItemEquipOption : MonoBehaviour {
    Item genItem;
    public ItemDictionary itemDictionary;

    public GameObject[] windowsToBeDisabled;


    public Renderer textMeshWindow;
    public string sortingLayerName;


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
                Debug.Log("Currently Equipped item: " + currentlyEquipped);
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
        foreach (GameObject windowAcc in windowsToBeDisabled)
        {
            windowAcc.SetActive(false);
        }
    }

    void Start()
    {
        textMeshWindow.sortingLayerName = sortingLayerName;
    }
}
