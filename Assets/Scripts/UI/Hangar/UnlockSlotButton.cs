using UnityEngine;
using System.Collections;

public class UnlockSlotButton : MonoBehaviour {
    public TextMesh creditReqDisplay;
    public TextMesh cogentumReqDisplay;
    int unlockCreditRequirement;
    int slotTracker;
    ArmoryControl armory;
    Item.ItemType curSlotType;

    public void initialize_slot(ArmoryControl controlCenter, int slotNum,
        int creditPrice, Item.ItemType slotType)
    {
        armory = controlCenter;
        unlockCreditRequirement = creditPrice;
        curSlotType = slotType;
        slotTracker = slotNum;
        creditReqDisplay.text = unlockCreditRequirement.ToString();
        cogentumReqDisplay.text = "0";
    }

    void Clicked()
    {
        //send message to control center with updated currency and slot access
        if (armory.get_owned_credit() >= unlockCreditRequirement)
            armory.unlock_slot(curSlotType, unlockCreditRequirement, 0);

    }
}
