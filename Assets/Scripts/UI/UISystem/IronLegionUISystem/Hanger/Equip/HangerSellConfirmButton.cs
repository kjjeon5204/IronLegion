using UnityEngine;
using System.Collections;

public class HangerSellConfirmButton : BaseUIButton {
    public HangerEquipmentControls hangerEquipControl;

    public enum ButtonFunctionality
    {
        SELL_CONFIRM,
        SELL_CANCEL
    }
    public ButtonFunctionality thisButtonFunction;

    public override void button_released_action()
    {
        if (thisButtonFunction == ButtonFunctionality.SELL_CONFIRM)
        {
            //Call sell confirmation function of hanger equip control
            
        }
        else if (thisButtonFunction == ButtonFunctionality.SELL_CANCEL)
        {
            //Disable confirmation window. Inform equip control that sell request has been canceled
            
        }
    }
}