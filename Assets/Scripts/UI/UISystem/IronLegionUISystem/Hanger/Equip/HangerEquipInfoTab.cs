using UnityEngine;
using System.Collections;

public class HangerEquipInfoTab : MonoBehaviour {
    public TextMesh itemNameDisplay;
    public TextMesh[] itemStatDisplay;
    public TextMesh[] itemStatTypeDisplay;
    public SpriteRenderer itemIconRenderer;
    public Sprite originalSprite;
    public Vector3 originalTransform;
    public Vector3 originalScale;

    public Vector3 modifiedTransform;
    public Vector3 modifiedScale;

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

    //Turns off all ui texts contained in itemStatsDisplay and itemStatTypeDisplay
    void deactivate_all_stat_text()
    {
        for (int ctr = 0; ctr < itemStatDisplay.Length; ctr++)
        {
            itemStatDisplay[ctr].gameObject.SetActive(false);
            itemStatTypeDisplay[ctr].gameObject.SetActive(false);
        }
    }

    public void set_item_stat(Item itemToDisplay)
    {
        deactivate_all_stat_text();
        if (itemToDisplay.itemID != "000000")
        {
            itemIconRenderer.sprite = itemToDisplay.gameObject.GetComponent<SpriteRenderer>().sprite;
            itemIconRenderer.gameObject.transform.localScale = modifiedScale;
            itemIconRenderer.gameObject.transform.localPosition = modifiedTransform;
            itemNameDisplay.text = itemToDisplay.itemName;
            int statCtr = 0;
            if (itemToDisplay.hp != 0)
            {
                itemStatDisplay[statCtr].gameObject.SetActive(true);
                itemStatTypeDisplay[statCtr].gameObject.SetActive(true);
                initialize_text(itemStatTypeDisplay[statCtr], itemStatDisplay[statCtr],
                    "HP: ", itemToDisplay.hp.ToString());
                statCtr++;
            }
            if (itemToDisplay.armor != 0)
            {
                itemStatDisplay[statCtr].gameObject.SetActive(true);
                itemStatTypeDisplay[statCtr].gameObject.SetActive(true);
                initialize_text(itemStatTypeDisplay[statCtr], itemStatDisplay[statCtr],
                    "Armor: ", itemToDisplay.armor.ToString());
                statCtr++;
            }
            if (itemToDisplay.damage != 0)
            {
                itemStatDisplay[statCtr].gameObject.SetActive(true);
                itemStatTypeDisplay[statCtr].gameObject.SetActive(true);
                initialize_text(itemStatTypeDisplay[statCtr], itemStatDisplay[statCtr],
                    "Damage: ", itemToDisplay.damage.ToString());
                statCtr++;
            }
            if (itemToDisplay.energy != 0)
            {
                itemStatDisplay[statCtr].gameObject.SetActive(true);
                itemStatTypeDisplay[statCtr].gameObject.SetActive(true);
                initialize_text(itemStatTypeDisplay[statCtr], itemStatDisplay[statCtr],
                    "Energy: ", itemToDisplay.energy.ToString());
                statCtr++;
            }
            if (itemToDisplay.penetration != 0)
            {
                itemStatDisplay[statCtr].gameObject.SetActive(true);
                itemStatTypeDisplay[statCtr].gameObject.SetActive(true);
                initialize_text(itemStatTypeDisplay[statCtr], itemStatDisplay[statCtr],
                    "Penetration: ", itemToDisplay.penetration.ToString());
                statCtr++;
            }
            if (itemToDisplay.luck != 0)
            {
                itemStatDisplay[statCtr].gameObject.SetActive(true);
                itemStatTypeDisplay[statCtr].gameObject.SetActive(true);
                initialize_text(itemStatTypeDisplay[statCtr], itemStatDisplay[statCtr],
                    "Luck: ", itemToDisplay.luck.ToString());
                statCtr++;
            }
        }
        else
        {
            itemIconRenderer.sprite = originalSprite;
            itemIconRenderer.gameObject.transform.localScale = originalScale;
            itemIconRenderer.gameObject.transform.localPosition = originalTransform;
            itemNameDisplay.text = "NONE";
            itemStatDisplay[0].text = "HP: 0";
            itemStatDisplay[1].text = "Armor: 0";
            itemStatDisplay[2].text = "Damage: 0";
            itemStatDisplay[3].text = "Energy: 0";
            itemStatDisplay[4].text = "Penetration: 0";
            itemStatDisplay[5].text = "Luck: 0";
        }
    }
}
