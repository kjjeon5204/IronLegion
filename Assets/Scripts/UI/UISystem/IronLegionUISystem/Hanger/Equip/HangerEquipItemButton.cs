using UnityEngine;
using System.Collections;

public class HangerEquipItemButton : BaseUIButton {
    public HangerEquipmentControls hangerEquipControls;

    public override void button_released_action()
    {
        hangerEquipControls.equip_item_logic();
    }
}
