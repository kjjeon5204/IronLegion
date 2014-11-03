using UnityEngine;
using System.Collections;

public class ItemBuyButton : MonoBehaviour {
    ArmoryControl armoryControl;
    ArmoryCatalog itemInformation;

    int creditRequired;
    int cogentumRequired;

    int creditOwned;
    int cogentumOwned;

    int slotNum;
    public GameObject soldButton;
    public ItemBuyConfirmButton itemBuyConfirmation;
    public ItemEquipOption itemEquipConfirmation;

    public void initialize_button(int slotCount, ArmoryControl inArmoryControl,
        ArmoryCatalog inItemInfo)
    {
        
        slotNum = slotCount;
        armoryControl = inArmoryControl;
        creditOwned = armoryControl.get_owned_credit();
        cogentumOwned = armoryControl.get_owned_cogentum();
        itemInformation = inItemInfo;
        Item itemInfo = itemInformation.itemObject.GetComponent<Item>();
        creditRequired = itemInfo.buy_price;
        cogentumRequired = itemInfo.cg_price;
        if (creditOwned < creditRequired || cogentumOwned < cogentumRequired)
        {
            SpriteRenderer curSprite = gameObject.GetComponent<SpriteRenderer>();
            Color myColor = curSprite.color;
            myColor.a = 0.5f;
            curSprite.color = myColor;
        }
        else
        {
            SpriteRenderer curSprite = gameObject.GetComponent<SpriteRenderer>();
            Color myColor = curSprite.color;
            myColor.a = 1.0f;
            curSprite.color = myColor;
        }
    }

    public void modify_button(int credit, int cogentum)
    {
        creditOwned = credit;
        cogentumOwned = cogentum;
        if (creditOwned < creditRequired || cogentumOwned < cogentumRequired)
        {
            SpriteRenderer curSprite = gameObject.GetComponent<SpriteRenderer>();
            Color myColor = curSprite.color;
            myColor.a = 0.5f;
            curSprite.color = myColor;
        }
        else
        {
            SpriteRenderer curSprite = gameObject.GetComponent<SpriteRenderer>();
            Color myColor = curSprite.color;
            myColor.a = 1.0f;
            curSprite.color = myColor;
        }
    }

    void Clicked()
    {
        creditOwned = armoryControl.get_owned_credit();
        cogentumOwned = armoryControl.get_owned_cogentum();
        if (creditOwned >= creditRequired &&
            cogentumOwned >= cogentumRequired)
        {
            armoryControl.item_bought(creditRequired, cogentumRequired, slotNum, 
                itemInformation.itemObject.GetComponent<Item>().itemType);
            Inventory playerInventory = new Inventory();
            playerInventory.load_inventory();
            playerInventory.add_item(itemInformation.itemObject.GetComponent<Item>().itemID);
            playerInventory.store_inventory();
        }
    }
}
