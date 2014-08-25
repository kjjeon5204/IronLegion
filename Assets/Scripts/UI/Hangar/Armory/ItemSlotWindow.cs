using UnityEngine;
using System.Collections;

public class ItemSlotWindow : MonoBehaviour {
    public TextMesh[] stats;
    public TextMesh itemName;
    public TextMesh creditPrice;
    public TextMesh cogentumPrice;
    public SpriteRenderer itemSpritePosition;
    public bool itemSold;
    public GameObject buyButton;
    public GameObject soldButton;
    Item curItemData;

    public void set_item_slot(ArmoryCatalog myData)
    {
        curItemData = myData.itemObject.GetComponent<Item>();
        itemName.text = curItemData.itemName;
        creditPrice.text = curItemData.buy_price.ToString();
        cogentumPrice.text = curItemData.cg_price.ToString();
        int statActivateCtr = 0;
        if (curItemData.damage != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = curItemData.damage.ToString();
            statActivateCtr++;
        }
        if (curItemData.armor != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = curItemData.armor.ToString();
            statActivateCtr++;
        }
        if (curItemData.hp != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = curItemData.hp.ToString();
            statActivateCtr++;
        }
        if (curItemData.energy != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = curItemData.energy.ToString();
            statActivateCtr++;
        }
        if (curItemData.luck != 0.0f)
        {
            stats[statActivateCtr].gameObject.SetActive(true);
            stats[statActivateCtr].text = curItemData.luck.ToString();
            statActivateCtr++;
        }
        itemSold = myData.itemSaleStatus;
        if (itemSold == true)
        {
            soldButton.SetActive(true);
            buyButton.SetActive(false);
        }
        else
        {
            soldButton.SetActive(false);
            buyButton.SetActive(true);
        }
    }
}
