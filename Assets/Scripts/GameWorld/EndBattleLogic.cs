using UnityEngine;
using System.Collections;

public class EndBattleLogic : MonoBehaviour
{
    bool isInitialized = false;

    public TextMesh missionStatusString;

    public TextMesh levelString;

    public TextMesh experienceString;

    public TextMesh creditString;

    public TextMesh itemNameString;

    public TextMesh[] itemStatString;

    public SpriteRenderer itemIcon;


    public void set_stat_text(TextMesh targetText, int stat, string statName)
    {
        if (stat > 0)
            targetText.text = statName + ": +" + stat.ToString();
        if (stat < 0)
            targetText.text = statName + ": " + stat.ToString();
    }

    

    public void initializeData(int creditReceived , PlayerLevelReadData inputData, GameObject itemPolled,
        bool battleWon)
    {
        if (battleWon == true)
        {
            missionStatusString.text = "Mission Accomplished!";
        }
        else
            missionStatusString.text = "Mission Failed!";
        levelString.text = "Level: " + inputData.level.ToString();
        experienceString.text = "Exp: " + inputData.curExperience + "/" + 
            inputData.expRequired;
        creditString.text = "Credits: " + creditReceived.ToString();

        Item curItem = itemPolled.GetComponent<Item>();
		Inventory myInventory = new Inventory();
		myInventory.load_inventory();
		myInventory.add_item(curItem.name);
		myInventory.store_inventory();
        itemNameString.text = curItem.itemName;

        itemIcon.sprite = itemPolled.GetComponent<SpriteRenderer>().sprite;

        int itemStatSlotAcc = 0;
        if (curItem.hp != 0)
        {
            set_stat_text(itemStatString[itemStatSlotAcc], curItem.hp, "HP");
            itemStatSlotAcc++;
        }

        if (curItem.armor != 0)
        {
            set_stat_text(itemStatString[itemStatSlotAcc], (int)curItem.armor, "Armor");
            itemStatSlotAcc++;
        }

        if (curItem.damage != 0)
        {
            set_stat_text(itemStatString[itemStatSlotAcc], (int)curItem.damage, "Damage");
            itemStatSlotAcc++;
        }

        if (curItem.energy != 0)
        {
            set_stat_text(itemStatString[itemStatSlotAcc], (int)curItem.energy, "Energy");
            itemStatSlotAcc++;
        }

        if (curItem.penetration != 0)
        {
            set_stat_text(itemStatString[itemStatSlotAcc], (int)curItem.penetration, "Penetration");
            itemStatSlotAcc++;
        }

        if (curItem.energy != 0)
        {
            set_stat_text(itemStatString[itemStatSlotAcc], (int)curItem.luck, "Luck");
            itemStatSlotAcc++;
        }

    }

    void Start()
    {
    }
}