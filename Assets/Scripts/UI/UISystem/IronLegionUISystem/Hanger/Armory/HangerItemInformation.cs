﻿using UnityEngine;
using System.Collections;

    

public class HangerItemInformation : MonoBehaviour {
    HangerArmoryController hangerArmoryControl;
    Item.ItemType itemWindowType;
    public GameObject lockedMessageText;
    public SpriteRenderer itemSpriteDisplay;
    public TextMesh itemName;
    public TextMesh[] statDescriptor;
    public TextMesh[] statDisplays;
    public TextMesh creditPriceDisplay;
    public TextMesh cogentumPriceDisplay;
    public bool itemSold;
    public bool itemSlotLocked;

    //Used to set to text mesh horizontally when the length of first text is 
    //unknown. This function shifts the second textmesh so that it follows right
    //after the first textmesh.
    void initialize_text(TextMesh inputDescriptor,
        TextMesh inputDisplay, string descriptor, string statVal)
    {
        inputDescriptor.text = descriptor;
        Vector3 statDisplayPosition = inputDescriptor.renderer.bounds.center;
        statDisplayPosition.x  += inputDescriptor.renderer.bounds.extents.x + 0.3f;

        inputDisplay.transform.position = statDisplayPosition;
        inputDisplay.text = statVal;
    }

    void deactivate_all_stat()
    {
        for (int ctr = 0; ctr < statDescriptor.Length; ctr++)
        {
            statDescriptor[ctr].gameObject.SetActive(false);
            statDisplays[ctr].gameObject.SetActive(false);
        }
    }

    public void empty_item_window()
    {
        deactivate_all_stat();
    }

    int purchaseCreditPrice;
    int purchaseCogentumPrice;

    //Used by the purchase button to purchase the item. This determines
    //whether the purchase was for an item or for unlocking the item window
    //and calls the correct function from Hanger Armory Controller
    public void item_purchased()
    {
        if (itemSlotLocked)
        {

        }
        else
        {

        }
    }

    //This function is used to set this window slot as a locked window. This functions assumes
    //that slot is locked slot and does not need to detect if it is truly locked or not.
    public void set_locked_item_window(int creditPrice, int cogentumPrice, Item.ItemType windowType, 
        HangerArmoryController inputController)
    {
        purchaseCreditPrice = creditPrice;
        purchaseCogentumPrice = cogentumPrice;
        hangerArmoryControl = inputController;
        cogentumPriceDisplay.text = cogentumPrice.ToString();
        creditPriceDisplay.text = creditPrice.ToString();
        deactivate_all_stat();
        itemName.text = "Locked";
        itemSlotLocked = true;
    }

    //This function is used to update the creditPrice and cogentumPrice of locked slot. This
    //function assumes that the slot is locked and only be called by controller if it is a locked
    //slot. This function is called when another window has been unlocked.
    public void update_locked_price(int creditPrice, int cogentumPrice)
    {
        purchaseCreditPrice = creditPrice;
        purchaseCogentumPrice = cogentumPrice;
        cogentumPriceDisplay.text = cogentumPrice.ToString();
        creditPriceDisplay.text = creditPrice.ToString();
    }
    
    //Used to set the item into the window. This function assumes that the slot is
    //an unlocked slot and will only be called by the controller only when it is unlocked.
    public void initialize_item_window(Item inputData, HangerArmoryController inputController)
    {
        lockedMessageText.SetActive(false);
        hangerArmoryControl = inputController;
        itemSlotLocked = false;
        deactivate_all_stat();
        itemName.text = inputData.itemName;
        itemSpriteDisplay.sprite = inputData.gameObject.GetComponent<SpriteRenderer>().sprite;
        itemSpriteDisplay.transform.localPosition = new Vector3(-0.72f, 0.81f, 0.0f);
        itemSpriteDisplay.transform.localScale = new Vector3(1.7f, 1.7f, 1.0f);

        //update prices
        purchaseCogentumPrice = inputData.cg_price;
        purchaseCreditPrice = inputData.buy_price;
        cogentumPriceDisplay.text = purchaseCogentumPrice.ToString();
        creditPriceDisplay.text = purchaseCreditPrice.ToString();

        //update stats
        int statAvailabilityCtr = 0;
        if (inputData.hp != 0)
        {
            statDescriptor[statAvailabilityCtr].gameObject.SetActive(true);
            statDisplays[statAvailabilityCtr].gameObject.SetActive(true);
            initialize_text(statDescriptor[statAvailabilityCtr], statDisplays[statAvailabilityCtr],
                "HP: ", inputData.hp.ToString());
            statAvailabilityCtr++;
        }
        if (inputData.damage != 0)
        {
            statDescriptor[statAvailabilityCtr].gameObject.SetActive(true);
            statDisplays[statAvailabilityCtr].gameObject.SetActive(true);
            initialize_text(statDescriptor[statAvailabilityCtr], statDisplays[statAvailabilityCtr],
                "Damage: ", inputData.damage.ToString());
            statAvailabilityCtr++;
        }
        if (inputData.armor != 0)
        {
            statDescriptor[statAvailabilityCtr].gameObject.SetActive(true);
            statDisplays[statAvailabilityCtr].gameObject.SetActive(true);
            initialize_text(statDescriptor[statAvailabilityCtr], statDisplays[statAvailabilityCtr],
                "Armor: ", inputData.armor.ToString());
            statAvailabilityCtr++;
        }
        if (inputData.energy != 0)
        {
            statDescriptor[statAvailabilityCtr].gameObject.SetActive(true);
            statDisplays[statAvailabilityCtr].gameObject.SetActive(true);
            initialize_text(statDescriptor[statAvailabilityCtr], statDisplays[statAvailabilityCtr],
                "Energy: ", inputData.energy.ToString());
            statAvailabilityCtr++;
        }
        if (inputData.penetration != 0)
        {
            statDescriptor[statAvailabilityCtr].gameObject.SetActive(true);
            statDisplays[statAvailabilityCtr].gameObject.SetActive(true);
            initialize_text(statDescriptor[statAvailabilityCtr], statDisplays[statAvailabilityCtr],
                "Armor Pen: ", inputData.penetration.ToString());
            statAvailabilityCtr++;
        }
        if (inputData.luck != 0)
        {
            statDescriptor[statAvailabilityCtr].gameObject.SetActive(true);
            statDisplays[statAvailabilityCtr].gameObject.SetActive(true);
            initialize_text(statDescriptor[statAvailabilityCtr], statDisplays[statAvailabilityCtr],
                "Luck: ", inputData.luck.ToString());
            statAvailabilityCtr++;
        }
    }
}
