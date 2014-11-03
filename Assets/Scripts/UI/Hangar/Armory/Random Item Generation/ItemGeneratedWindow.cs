using UnityEngine;
using System.Collections;

public class ItemGeneratedWindow : MonoBehaviour {
    public SpriteRenderer itemDisplay;
    public TextMesh[] stats;
    public TextMesh[] changedStat;
    public TextMesh itemName;

    public ItemBuyConfirmButton itemBuyConfirm;
    public ItemEquipOption itemEquipConfirm;


    int modifiedAttack;
    int modifiedArmor;
    int modifiedHealth;
    int modifiedEnergy;
    int modifiedluck;
    int modifiedPenetration;

    public Renderer[] myTexts;

    public PlayerMasterData playerMasterData;
    public ItemDictionary itemDictionary;
    Item[] itemList;
    bool initialized = false;

    void initialize_table()
    {
        string[] equippedItemStringList = playerMasterData.access_equipment_data().get_equipped_item();
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

    public void display_generated_item(Item curItemData)
    {
        initialize_table();

        if (curItemData.itemType == Item.ItemType.HEAD)
        {
            get_modified_stat(itemList[0], curItemData);
        }
        else if (curItemData.itemType == Item.ItemType.WEAPON)
        {
            get_modified_stat(itemList[1], curItemData);
        }
        else if (curItemData.itemType == Item.ItemType.ARMOR)
        {
            get_modified_stat(itemList[2], curItemData);
        }
        else if (curItemData.itemType == Item.ItemType.CORE)
        {
            if (itemList[3] != null && itemList[4] != null &&
                itemList[3].energy > itemList[4].energy)
            {
                get_modified_stat(itemList[3], curItemData);
            }
            else
            {
                //Debug.Log("Check player data");
                get_modified_stat(itemList[4], curItemData);
            }
        }

        foreach (Renderer myText in myTexts)
        {
            myText.sortingLayerName = "ConfirmWindow";
        }

        itemDisplay = curItemData.GetComponent<SpriteRenderer>();
        int statActivateCtr = 0;
        itemName.text = curItemData.itemName;
        if (curItemData.damage != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = "ATK " + curItemData.damage.ToString();
            statActivateCtr++;
        }
        if (curItemData.armor != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = "DEF " + curItemData.armor.ToString();
            statActivateCtr++;
        }
        if (curItemData.hp != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = "HP " + curItemData.hp.ToString();
            statActivateCtr++;
        }
        if (curItemData.energy != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = "EN " + curItemData.energy.ToString();
            statActivateCtr++;
        }
        if (curItemData.luck != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = "LUK " + curItemData.luck.ToString();
            statActivateCtr++;
        }
        if (curItemData.penetration != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = "PEN " + curItemData.luck.ToString();
            statActivateCtr++;
        }
        statActivateCtr = 0;
        if (modifiedAttack != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (modifiedAttack > 0.0f)
                changedStat[statActivateCtr].text = "ATK +" + modifiedAttack;
            else
                changedStat[statActivateCtr].text = "ATK " + modifiedAttack;
            statActivateCtr++;
        }
        if (modifiedArmor != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (modifiedArmor > 0.0f)
                changedStat[statActivateCtr].text = "DEF +" + modifiedArmor;
            else
                changedStat[statActivateCtr].text = "DEF " + modifiedArmor;
            statActivateCtr++;
        }
        if (modifiedHealth != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (modifiedHealth > 0.0f)
                changedStat[statActivateCtr].text = "HP +" + modifiedHealth;
            else
                changedStat[statActivateCtr].text = "HP " + modifiedHealth;
            statActivateCtr++;
        }
        if (modifiedEnergy != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (modifiedEnergy > 0.0f)
                changedStat[statActivateCtr].text = "EN +" + modifiedEnergy;
            else
                changedStat[statActivateCtr].text = "EN " + modifiedEnergy;
            statActivateCtr++;
        }
        if (modifiedluck != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (modifiedluck > 0.0f)
                changedStat[statActivateCtr].text = "LUK +" + modifiedluck;
            else
                changedStat[statActivateCtr].text = "LUK " + modifiedluck;
            statActivateCtr++;
        }
        if (modifiedPenetration != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (modifiedPenetration > 0.0f)
                changedStat[statActivateCtr].text = "PEN +" + modifiedPenetration;
            else
                changedStat[statActivateCtr].text = "PEN " + modifiedPenetration;
            statActivateCtr++;
        }
        itemBuyConfirm.set_confirmation_button(curItemData);
        itemEquipConfirm.set_generated_item(curItemData);
    }
}
