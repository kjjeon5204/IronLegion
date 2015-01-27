using UnityEngine;
using System.Collections;

public class HangerArmorySlotButton : BaseUIButton {
    public HangerArmoryController armoryControl;

    public enum StoreWindowType
    {
        HEAD = 0,
        WEAPON = 1,
        ARMOR = 2,
        CORE = 3,
        UPGRADE = 4
    }
    public StoreWindowType myStoreWindow;


    public override void button_released_action()
    {
        armoryControl.store_window_operator((int)myStoreWindow);
    }
}
