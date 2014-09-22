using UnityEngine;
using System.Collections;

public class ItemGeneratedWindow : MonoBehaviour {
    public SpriteRenderer itemDisplay;
    public TextMesh[] stats;
    public TextMesh[] changedStat;

    public ItemBuyConfirmButton itemBuyConfirm;
    public ItemEquipOption itemEquipConfirm;


    float damageChange;
    float armorChange;
    float healthChange;
    float energyChange;
    float luckChange;

    public Renderer[] myTexts;
    

    public void display_generated_item(Item curItemData)
    {
        foreach (Renderer myText in myTexts)
        {
            myText.sortingLayerName = "ConfirmWindow";
        }

        itemDisplay = curItemData.GetComponent<SpriteRenderer>();
        int statActivateCtr = 0;
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
        statActivateCtr = 0;
        if (damageChange != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (damageChange > 0.0f)
                changedStat[statActivateCtr].text = "ATK +" + damageChange;
            else
                changedStat[statActivateCtr].text = "ATK " + damageChange;
            statActivateCtr++;
        }
        if (armorChange != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (armorChange > 0.0f)
                changedStat[statActivateCtr].text = "DEF +" + armorChange;
            else
                changedStat[statActivateCtr].text = "DEF " + armorChange;
            statActivateCtr++;
        }
        if (healthChange != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (healthChange > 0.0f)
                changedStat[statActivateCtr].text = "HP +" + healthChange;
            else
                changedStat[statActivateCtr].text = "HP " + healthChange;
            statActivateCtr++;
        }
        if (energyChange != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (energyChange > 0.0f)
                changedStat[statActivateCtr].text = "EN +" + energyChange;
            else
                changedStat[statActivateCtr].text = "EN " + energyChange;
            statActivateCtr++;
        }
        if (luckChange != 0.0f)
        {
            changedStat[statActivateCtr].gameObject.SetActive(true);
            if (luckChange > 0.0f)
                changedStat[statActivateCtr].text = "LUK +" + luckChange;
            else
                changedStat[statActivateCtr].text = "LUK " + luckChange;
            statActivateCtr++;
        }
        itemBuyConfirm.set_confirmation_button(curItemData);
        itemEquipConfirm.set_generated_item(curItemData);
    }
}
