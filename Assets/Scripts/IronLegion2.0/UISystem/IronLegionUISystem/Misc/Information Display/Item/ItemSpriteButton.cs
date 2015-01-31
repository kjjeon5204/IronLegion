using UnityEngine;
using System.Collections;

public class ItemSpriteButton : BaseUIButton {
    public HangerEquipInfoTab itemInfoSlot;
    public Item itemInfo;
    

    public override void button_pressed_action()
    {
        itemInfoSlot.gameObject.SetActive(true);
        itemInfoSlot.set_item_stat(itemInfo);
    }

    public void set_item(Item itemInput)
    {
        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.sprite = itemInput.gameObject.GetComponent<SpriteRenderer>().sprite;
        itemInfo = itemInput;
    }
}
