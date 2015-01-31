using UnityEngine;
using System.Collections;

public class HangerEquipSlots : BaseUIButton {
    public HangerEquipmentControls hangerEquipControls;
    public SpriteRenderer itemSpriteRenderer;
    public Item.ItemType curSlotType;
    Item itemCurEquipped;

    //Used to set equipment item. inputItem specifies the item being equipped
    //and after equipping, returns the item that was previously equipped. Returns
    //a null item if there was no item previously equipped
    public Item set_equipment_window(Item inputItem)
    {
        Item temp = itemCurEquipped;
        itemCurEquipped = inputItem;
        itemSpriteRenderer.sprite = inputItem.gameObject.GetComponent<SpriteRenderer>().sprite;
        itemSpriteRenderer.gameObject.transform.localPosition = new Vector3(-0.736f, 0.736f, 0.0f);
        itemSpriteRenderer.gameObject.transform.localScale = new Vector3(1.633f, 1.633f, 1.0f);
        return temp;
    }

    public Item get_equipped_item()
    {
        return itemCurEquipped;
    }


    //Used to send hanger equipment slot select event to hanger equip controls
    //This function is called from UI2DController when this button is preassed
    public override void button_pressed_action()
    {
        hangerEquipControls.on_equip_slot_press(curSlotType, itemCurEquipped);
    }
}
