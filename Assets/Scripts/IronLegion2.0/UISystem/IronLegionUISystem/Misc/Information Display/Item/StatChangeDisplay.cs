using UnityEngine;
using System.Collections;

//This class is used to display the stat change that takes place in 
//player character after item is switched
public class StatChangeDisplay : MonoBehaviour {
    public TextMesh[] statText;
    public TextMesh[] statModText;
    public TextMesh playerMechNameDisplay;
    public Color statDecreaseTextColor;
    public Color statIncreaseTextColor;

    void initialize_text(TextMesh inputDescriptor,
        TextMesh inputDisplay, string descriptor, string statVal)
    {
        inputDescriptor.text = descriptor;
        Vector3 statDisplayPosition = inputDisplay.transform.position;
        statDisplayPosition.x = inputDescriptor.renderer.bounds.center.x;
        statDisplayPosition.x += inputDescriptor.renderer.bounds.extents.x + 0.1f;

        inputDisplay.transform.position = statDisplayPosition;
        inputDisplay.text = statVal;
    }

    void setup_text(int statDisplayRef, string statDesc, 
        int statVal, int statChangeVal)
    {
        string statChangeString;
        if (statChangeVal != 0)
        {

            Debug.Log(statChangeVal);
            statModText[statDisplayRef].gameObject.SetActive(true);
            if (statChangeVal > 0)
            {
                statChangeString = "(+" + statChangeVal + ")";
                statModText[statDisplayRef].color = statIncreaseTextColor;
            }
            else
            {
                statChangeString = "(-" + statChangeVal + ")";
                statModText[statDisplayRef].color = statDecreaseTextColor;
            }
            initialize_text(statText[statDisplayRef], statModText[statDisplayRef],
                 statDesc + ": " + statVal.ToString(), statChangeString);
        }
        else
        {
            statText[statDisplayRef].text = statDesc + ": " + statVal;
            statModText[statDisplayRef].gameObject.SetActive(false);
        }

    }

    public void display_player_stats()
    {
        PlayerMechData curMechData = new PlayerMechData();
        curMechData.health = 500;
        curMechData.armor = 15;
        curMechData.damage = 100;
        curMechData.pentration = 10;
        curMechData.energy = 100;
        curMechData.luck = 5;
        curMechData.mechID = "Odin";
        if (playerMechNameDisplay != null)
            playerMechNameDisplay.text = curMechData.mechID;
        setup_text(0, "HP", curMechData.health, 0);
        setup_text(1, "Armor", curMechData.armor, 0);
        setup_text(2, "Damage", curMechData.damage, 0);
        setup_text(3, "Pen", curMechData.pentration, 0);
        setup_text(4, "Energy", curMechData.energy, 0);
        setup_text(5, "Luck", curMechData.luck, 0);
    }

    public void check_item_switch(Item itemToEquip, Item itemEquipped)
    {
        int hpChange;
        int armorChange;
        int damageChange;
        int penChange;
        int energyChange;
        int luckChange;
        if (itemEquipped == null)
        {
            //No item is equipped in the referenced spot.
            //Item stat changes are reflected directly
            hpChange = itemToEquip.hp;
            armorChange = itemToEquip.armor;
            damageChange = itemToEquip.damage;
            penChange = itemToEquip.penetration;
            energyChange = itemToEquip.energy;
            luckChange = itemToEquip.luck;
        }
        else
        {
            hpChange = itemToEquip.hp - itemEquipped.hp;
            armorChange = itemToEquip.armor - itemEquipped.armor;
            damageChange = itemToEquip.damage - itemEquipped.damage;
            penChange = itemToEquip.penetration - itemEquipped.penetration;
            energyChange = itemToEquip.energy - itemEquipped.energy;
            luckChange = itemToEquip.luck - itemEquipped.luck;
        }
        PlayerMechData curMechData = new PlayerMechData();
        curMechData.health = 500;
        curMechData.armor = 15;
        curMechData.damage = 100;
        curMechData.pentration = 10;
        curMechData.energy = 100;
        curMechData.luck = 5;
        curMechData.mechID = "Odin";
        if (playerMechNameDisplay != null)
            playerMechNameDisplay.text = curMechData.mechID;
        setup_text(0, "HP", curMechData.health, hpChange);
        setup_text(1, "Armor", curMechData.armor, armorChange);
        setup_text(2, "Damage", curMechData.damage, damageChange);
        setup_text(3, "Pen", curMechData.pentration, penChange);
        setup_text(4, "Energy", curMechData.energy, energyChange);
        setup_text(5, "Luck", curMechData.luck, luckChange);
    }
}
