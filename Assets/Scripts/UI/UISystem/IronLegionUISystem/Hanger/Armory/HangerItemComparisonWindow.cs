using UnityEngine;
using System.Collections;

public class HangerItemComparisonWindow : MonoBehaviour {
    public ItemDictionary itemDictionary;
    public TextMesh[] currentlyEquippedStats;
    public TextMesh[] modifiedStat;

    public Color statDecreaseColor;
    public Color statIncreaseColor;

    int hpModified;
    int damageModified;
    int armorModified;
    int energyModified;
    int damagePenModified;
    int luckModified;


    public void compare_stats(Item itemEquipped, Item itemToEquip)
    {
        hpModified = itemToEquip.hp - itemEquipped.hp;
        damageModified = itemToEquip.damage - itemEquipped.damage;
        armorModified = itemToEquip.armor - itemEquipped.armor;
        energyModified = itemToEquip.energy - itemEquipped.energy;
        damagePenModified = itemToEquip.penetration - itemEquipped.penetration;
        luckModified = itemToEquip.luck - itemEquipped.luck;
    }

	// Use this for initialization
	void Start () {
	
	}

    public void display_modified_stat(Item itemEquipped, Item itemToEquip)
    {
        compare_stats(itemEquipped, itemToEquip);
        int statDisplayCtr = 0;
        if (hpModified != 0)
        {
            modifiedStat[statDisplayCtr].gameObject.SetActive(true);
            modifiedStat[statDisplayCtr].text = hpModified.ToString();
            if (hpModified > 0)
            {
                modifiedStat[statDisplayCtr].color = statIncreaseColor;
            }
            if (hpModified < 0)
            {
                modifiedStat[statDisplayCtr].color = statDecreaseColor;
            }
            statDisplayCtr++;
        }
        if (damageModified != 0)
        {
            modifiedStat[statDisplayCtr].gameObject.SetActive(true);
            modifiedStat[statDisplayCtr].text = damageModified.ToString();
            if (damageModified > 0)
            {
                modifiedStat[statDisplayCtr].color = statIncreaseColor;
            }
            if (damageModified < 0)
            {
                modifiedStat[statDisplayCtr].color = statDecreaseColor;
            }
            statDisplayCtr++;
        }
        if (armorModified != 0)
        {
            modifiedStat[statDisplayCtr].gameObject.SetActive(true);
            modifiedStat[statDisplayCtr].text = armorModified.ToString();
            if (armorModified > 0)
            {
                modifiedStat[statDisplayCtr].color = statIncreaseColor;
            }
            if (armorModified < 0)
            {
                modifiedStat[statDisplayCtr].color = statDecreaseColor;
            }
            statDisplayCtr++;
        }
        if (energyModified != 0)
        {
            modifiedStat[statDisplayCtr].gameObject.SetActive(true);
            modifiedStat[statDisplayCtr].text = energyModified.ToString();
            if (energyModified > 0)
            {
                modifiedStat[statDisplayCtr].color = statIncreaseColor;
            }
            if (energyModified < 0)
            {
                modifiedStat[statDisplayCtr].color = statDecreaseColor;
            }
            statDisplayCtr++;
        }
        if (damagePenModified != 0) 
        {
            modifiedStat[statDisplayCtr].gameObject.SetActive(true);
            modifiedStat[statDisplayCtr].text = damagePenModified.ToString();
            if (damagePenModified > 0)
            {
                modifiedStat[statDisplayCtr].color = statIncreaseColor;
            }
            if (damagePenModified < 0)
            {
                modifiedStat[statDisplayCtr].color = statDecreaseColor;
            }
            statDisplayCtr++;
        }
        if (luckModified != 0)
        {
            modifiedStat[statDisplayCtr].gameObject.SetActive(true);
            modifiedStat[statDisplayCtr].text = luckModified.ToString();
            if (luckModified > 0)
            {
                modifiedStat[statDisplayCtr].color = statIncreaseColor;
            }
            if (luckModified < 0)
            {
                modifiedStat[statDisplayCtr].color = statDecreaseColor;
            }
            statDisplayCtr++;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
