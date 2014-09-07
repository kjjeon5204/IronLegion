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
        itemInformation = inItemInfo;
        Item itemInfo = itemInformation.itemObject.GetComponent<Item>();
        creditRequired = itemInfo.buy_price;
        cogentumRequired = itemInfo.cg_price;
    }

    public void modify_button(int credit, int cogentum)
    {
        creditOwned = credit;
        cogentumOwned = cogentum;
    }

    void Clicked()
    {
        creditOwned = armoryControl.get_owned_credit();
        cogentumOwned = armoryControl.get_owned_cogentum();
        if (creditOwned > creditRequired &&
            cogentumOwned > cogentumRequired)
        {
            Debug.Log("Buy Item");
            armoryControl.item_bought(creditRequired, cogentumRequired, slotNum, 
                itemInformation.itemObject.GetComponent<Item>().itemType);
            Inventory playerInventory = new Inventory();
            playerInventory.load_inventory();
            playerInventory.add_item(itemInformation.itemObject.GetComponent<Item>().itemID);
            playerInventory.store_inventory();
        }
    }
}
