using UnityEngine;
using System.Collections;

public class HangerEquipItemSlots : BaseUIButton {
    public SpriteRenderer itemSpriteDisplay;
    HangerEquipmentControls hangerEquipControls;
    Item item;
    int equipAccess;

    public void set_current_item(Item inputItem, int inEquipAccess,HangerEquipmentControls inputController)
    {
        hangerEquipControls = inputController;
        itemSpriteDisplay.sprite = inputItem.gameObject
            .GetComponent<SpriteRenderer>().sprite;
        item = inputItem;
        equipAccess = inEquipAccess;
    }

    public override void button_released_action()
    {
        hangerEquipControls.item_selected(item, equipAccess);
    }
}
