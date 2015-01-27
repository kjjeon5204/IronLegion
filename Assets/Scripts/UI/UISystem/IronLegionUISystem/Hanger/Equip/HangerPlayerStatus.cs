using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//This class handles the player experience bar and player mech information updates.
public class HangerPlayerStatus : MonoBehaviour {
    public GameObject playerExpBar;
    public HeroLevelData playerLevelData;
    public TextMesh[] statTexts;
    public ItemDictionary itemDictionary;
    public SpriteRenderer[] equipSlotUI;
    public TextMesh playerExperienceText;


    //Used to update the player data.
    public void update_player_data()
    {
        PlayerMechData currentlyEquippedMech = UserData.userDataContainer.get_current_mech();
        int hp = playerLevelData.get_player_level_data(currentlyEquippedMech.level).HP 
            + currentlyEquippedMech.health;
        int damage = (int)playerLevelData.get_player_level_data(currentlyEquippedMech.level).damage
            + currentlyEquippedMech.damage;
        int armor = currentlyEquippedMech.armor;
        int armorPen = currentlyEquippedMech.pentration;
        int energy = 100 + currentlyEquippedMech.energy;
        int luck = currentlyEquippedMech.luck;
        int experience = playerLevelData.get_player_level_data(currentlyEquippedMech.level).experience;

        playerExperienceText.text = currentlyEquippedMech.curExp + "/" + experience;

        statTexts[0].text = currentlyEquippedMech.level.ToString();
        statTexts[1].text = hp.ToString();
        statTexts[2].text = armor.ToString();
        statTexts[3].text = damage.ToString();
        statTexts[4].text = energy.ToString();
        statTexts[5].text = armorPen.ToString();
        statTexts[6].text = luck.ToString();

        /*
        IList<string> currentEquippedItem = currentlyEquippedMech.itemsEquipped;
        for (int ctr = 0; ctr < 5; ctr++)
        {
            if (currentEquippedItem[ctr] != "000000")
            {
                Item currentItem = itemDictionary.get_item_data(currentEquippedItem[ctr]).GetComponent<Item>();
                equipSlotUI[ctr].sprite = currentItem.GetComponent<SpriteRenderer>().sprite;
            }
        }
         */ 
    }
}
