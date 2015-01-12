using UnityEngine;
using System.Collections;

public class HangerArmoryBuyConfirm : MonoBehaviour {
    //Item buy confirmation
    public HangerArmoryController hangerArmoryController;
    public SpriteRenderer itemSprite;
    public TextMesh buyConfirmMessage;
    public GameObject[] itemBuyButtons;
    int equipSlotStore;
    Item itemStore;

    //Used to initialize/set confirmation window with item and confirm message.
    //item represents item being bought and equipSlot is used for Core type items.
    public void set_item_to_buy(Item item, int equipSlot)
    {
        //get sprite
        itemSprite.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
        //display message
        buyConfirmMessage.text = "Are you sure you want to buy /n" + item.itemName + "?";
        equipSlotStore = equipSlot;
        itemStore = item;
    }


    //Called by the confirm button. 
    public void confirm_item_buy(HangerArmoryController.TransactionOption transactionOption)
    {
        
        hangerArmoryController.complete_trasaction_process(itemStore, transactionOption, equipSlotStore);
    }



    //Slot unlock confirmation
    public TextMesh unlockSlotMessage;
    public GameObject[] unlockSlotButtons;
    HangerItemInformation currentHangerWindowToUnlock;

    public void set_unlock_slot_message(HangerItemInformation slotToUnlock, int credit)
    {
        //Disable all item buy information display
        itemSprite.gameObject.SetActive(false);
        buyConfirmMessage.gameObject.SetActive(false);
        for (int ctr = 0; ctr < itemBuyButtons.Length; ctr++)
        {
            itemBuyButtons[ctr].SetActive(false);
        }
        //Display unlock confirm message
        //Enable all unlock slot buttons
        for (int ctr = 0; ctr < unlockSlotButtons.Length; ctr++)
        {
            itemBuyButtons[ctr].SetActive(true);
        }
        unlockSlotMessage.gameObject.SetActive(true);
        currentHangerWindowToUnlock = slotToUnlock;
    }

    //
    public void complete_unlock_slot ()
    {
        hangerArmoryController.complete_unlock_window(currentHangerWindowToUnlock);
    }
}
