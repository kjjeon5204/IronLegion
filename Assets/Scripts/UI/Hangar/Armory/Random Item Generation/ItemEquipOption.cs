using UnityEngine;
using System.Collections;

public class ItemEquipOption : MonoBehaviour {
    Item genItem;
    public ItemDictionary itemDictionary;

    public GameObject[] windowsToBeDisabled;


    public Renderer textMeshWindow;
    public string sortingLayerName;
    public PlayerMasterData playerMasterData;

    public void set_generated_item(Item generatedItem)
    {
        genItem = generatedItem;
    }

    void Clicked()
    {
        string currentlyEquipped = "000000";

        Debug.Log(genItem.itemID + " " + genItem.itemName);
        if (genItem.itemID[2] == 'H')
        {
            currentlyEquipped = playerMasterData.access_equipment_data().get_equipped_item(0);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                playerMasterData.access_equipment_data().remove_item(curEquippedItem);
                playerMasterData.add_item(currentlyEquipped);
            }
            playerMasterData.access_equipment_data().equip_item(genItem);
        }
        else if (genItem.itemID[2] == 'C')
        {
            currentlyEquipped = playerMasterData.access_equipment_data().get_equipped_item(3);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                playerMasterData.access_equipment_data().remove_item(curEquippedItem);
                playerMasterData.add_item(currentlyEquipped);
            }
            playerMasterData.access_equipment_data().equip_item(genItem);
        }
        else if (genItem.itemID[2] == 'B')
        {
            currentlyEquipped = playerMasterData.access_equipment_data().get_equipped_item(2);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                playerMasterData.access_equipment_data().remove_item(curEquippedItem);
                playerMasterData.add_item(currentlyEquipped);
            }
            playerMasterData.access_equipment_data().equip_item(genItem);
        }
        else if (genItem.itemID[2] == 'W')
        {
            currentlyEquipped = playerMasterData.access_equipment_data().get_equipped_item(1);
            if (currentlyEquipped != "000000")
            {
                Item curEquippedItem = itemDictionary.get_item_data(currentlyEquipped).GetComponent<Item>();
                playerMasterData.access_equipment_data().remove_item(curEquippedItem);
                playerMasterData.add_item(currentlyEquipped);
            }
            playerMasterData.access_equipment_data().equip_item(genItem);
        }
        playerMasterData.store_inventory();
        playerMasterData.save_hero_equip_data();
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
