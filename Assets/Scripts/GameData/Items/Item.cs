using UnityEngine;
using System.Collections;




public class Item : MonoBehaviour {
    public static Item nullItem;
    public static void initialize_null_item()
    {
        nullItem = new Item();
        nullItem.itemName = "None";
        nullItem.itemID = "000000";
        nullItem.hp = 0;
        nullItem.armor = 0;
        nullItem.damage = 0;
        nullItem.energy = 0;
        nullItem.penetration = 0;
        nullItem.luck = 0;
        nullItem.buy_price = 0;
        nullItem.cg_price = 0;
        nullItem.sell_price = 0;
    }

    public enum ItemRarity
    {
        COMMON,
        RARE,
        LEGENDARY,
        UNCOMMON
    }

    public enum ItemType
    {
        HEAD,
        WEAPON,
        ARMOR,
        CORE
    }


    /*Store item data*/

    public string itemID;
    public string itemName;
    public ItemRarity itemRarity;
	public ItemType itemType;
    public int hp;
    public int armor;
    public int damage;
    public int energy;
    public int penetration;
    public int luck;
	public int buy_price;
	public int cg_price;
	public int sell_price;
}
