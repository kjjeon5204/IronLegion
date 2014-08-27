using UnityEngine;
using System.Collections;




public class Item : MonoBehaviour {
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
    public float armor;
    public float damage;
    public float energy;
    public float penetration;
    public float luck;
	public int buy_price;
	public int cg_price;
	public int sell_price;
}
