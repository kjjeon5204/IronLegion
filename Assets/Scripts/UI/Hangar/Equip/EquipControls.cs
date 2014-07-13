using UnityEngine;
using System.Collections;

public class EquipControls : MonoBehaviour {
	
	public InventorySlot inventory_to_switch;
	public EquippedSlot equipped_to_switch;
	
	public Item item_inventory;
	public Item item_equipped;
	
	public bool swappable;
	private ItemControls inventory;
	
	SpriteRenderer image;
	// Use this for initialization
	void Awake () {
		image = GetComponent<SpriteRenderer>();
		inventory = GameObject.Find("InventoryStart").GetComponent<ItemControls>();
	}
	
	// Update is called once per frame
	void Update () {
		if ( CheckSwappable() )
		{
			image.color = Color.green;
			swappable = true;
		}
		else
		{
			image.color = Color.red;
			swappable = false;
		}
	}
	
	void Clicked() {
		if (swappable)
		{
			string temp;
			temp = inventory_to_switch.item_id;
			inventory_to_switch.item_id = equipped_to_switch.item_id;
			equipped_to_switch.item_id = temp;
			Item item_temp = new Item();
			item_temp = item_inventory;
			item_inventory = item_equipped;
			item_equipped = item_temp;
			
			inventory.UpdateEquipped();
			inventory.UpdateInventory();
			swappable = false;
		}
	}
	
	bool CheckSwappable() {
		if (item_inventory == null || item_equipped == null)
		return false;
		if (item_inventory.itemID == "000000" && item_equipped.itemID == "000000")
		return false;
		else if  (item_inventory.itemID == "000000" && item_equipped.itemID != "000000")
		{
			return true;
		}
		else if (item_inventory.itemID != "000000")
		{
			if (item_equipped.itemType == item_inventory.itemType)
			return true;
			else
			return false;
		}
		return false;
	}
}
