using UnityEngine;
using System.Collections;

public class ItemSlotWindow : MonoBehaviour {
    ArmoryCatalog curCatalog;
    public TextMesh[] stats;
    public TextMesh itemName;
    public TextMesh creditPrice;
    public TextMesh cogentumPrice;
    public SpriteRenderer itemSpritePosition;
    public bool itemSold;
    public GameObject buyButton;
    public GameObject soldButton;
    Item curItemData;
    ItemDisplayWindow itemDisplayWindow;
    public ArmoryControl armoryControl;
    public ItemBuyButton itemBuyButton;
    int slotNum;

    int creditOwned;
    int cogentumOwned;


    public void set_item_slot(ArmoryCatalog myData, ArmoryControl inAmoryControl, int slotNumInput)
    {
        armoryControl = inAmoryControl;
        curCatalog = myData;
        curItemData = myData.itemObject.GetComponent<Item>();
        itemSpritePosition.sprite = myData.itemObject.GetComponent<SpriteRenderer>().sprite;
        itemName.text = curItemData.itemName;
        creditPrice.text = curItemData.buy_price.ToString();
        cogentumPrice.text = curItemData.cg_price.ToString();
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
        slotNum = slotNumInput;
        itemBuyButton.initialize_button(slotNum, armoryControl,
            curCatalog);
    }

    public void update_sale_status(bool input) {
        itemSold = input;
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

    public void currency_update(int credit, int cogentum)
    {
        creditOwned = credit;
        cogentumOwned = cogentum;

        
    }

    public void custom_clicked()
    {
        armoryControl.set_selectedItem(curCatalog.itemObject);
    }


    void Clicked()
    {
        armoryControl.set_selectedItem(curCatalog.itemObject);
    }


    public ArmoryCatalog buy_item()
    {
        itemSold = true;
        curCatalog.itemSaleStatus = true;
        return curCatalog;
    }

    public ArmoryCatalog get_slot_info()
    {
        return curCatalog;
    }
}
