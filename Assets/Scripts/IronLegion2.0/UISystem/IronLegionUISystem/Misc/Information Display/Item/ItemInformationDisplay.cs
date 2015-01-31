using UnityEngine;
using System.Collections;

public class ItemInformationDisplay : MonoBehaviour {
    public TextMesh[] statDisplay;
    public TextMesh itemName;

    //Optional displays
    public SpriteRenderer itemSpriteDisplay;

    public void set_item_info(Item item)
    {
        if (item == null)
            item = Item.nullItem;
        itemName.text = item.itemName;
        statDisplay[0].text = "HP: " + item.hp.ToString();
        statDisplay[1].text = "Armor: " + item.armor.ToString();
        statDisplay[2].text = "Damage: " + item.damage.ToString();
        statDisplay[3].text = "Pen: " + item.damage.ToString();
        statDisplay[4].text = "Energy: " + item.energy.ToString();
        statDisplay[5].text = "Luck: " + item.luck.ToString();

        if (itemSpriteDisplay != null)
            itemSpriteDisplay.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
        
    }
}
