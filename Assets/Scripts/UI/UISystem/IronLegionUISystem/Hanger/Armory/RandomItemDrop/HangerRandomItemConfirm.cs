using UnityEngine;
using System.Collections;

public class HangerRandomItemConfirm : MonoBehaviour {
    public TextMesh[] statDescriptionDisplay;
    public TextMesh[] statDisplay;
    public TextMesh itemNameDisplay;

    public TextMesh[] statDiff;

    public Color statIncreaseColor;
    public Color statDescreaseColor;



    float hpDiff;
    float armorDiff;
    float damageDiff;
    float penetrationDiff;
    float energyDiff;
    float luckDiff;

    public TextMesh[] charStatDescription;
    public  

    int equipSlot;

    public void set_equip_slot(int slot)
    {
        equipSlot = slot;
    }

    public ItemDictionary itemDictionary;

    /*This function retrieves the item obtained and compares the stat with
     currently equipped slot.*/
    void calculate_stat_difference(Item itemObtained)
    {
        string equippedItemID = UserData.userDataContainer.get_current_mech().itemsEquipped[equipSlot];
        GameObject equippedItemObject = itemDictionary.get_item_data(equippedItemID);
        if (equippedItemObject != null)
        {
            Item itemEquipped = equippedItemObject.GetComponent<Item>();
            hpDiff = itemObtained.hp - itemEquipped.hp;
            armorDiff = itemObtained.armor - itemEquipped.armor;
            damageDiff = itemObtained.damage - itemEquipped.damage;
            penetrationDiff = itemObtained.penetration - itemEquipped.penetration;
            energyDiff = itemObtained.energy - itemEquipped.energy;
            luckDiff = itemObtained.luck - itemEquipped.luck;
        }
        else
        {
            hpDiff = itemObtained.hp;
            armorDiff = itemObtained.armor;
            damageDiff = itemObtained.damage;
            penetrationDiff = itemObtained.penetration;
            energyDiff = itemObtained.energy;
            luckDiff = itemObtained.luck;
        }
        
    }

    /*Used to initialize text format where Stat number is displayed right after 
     the stat number.*/
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

    /*This function compares player currently equipped item with item generated.
     If the item is Core, it generates a toggle which allows player to select the
     slot that the player wants to compare to and equip.*/
    public void set_random_item_confirm(Item item)
    {
        //Display generated item's stats
        itemNameDisplay.text = item.itemName;

    }
}
