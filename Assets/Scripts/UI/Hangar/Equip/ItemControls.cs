using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct ItemList
{
	public GameObject[] items;
}

public class ItemControls : MonoBehaviour {
	public ItemList[] item_tier;

	//public GameObject[] items;
	public GameObject inventory_slot;
	private Inventory inventory;
	public int num_of_items;
	public GameObject empty_slot;
	
	private HeroStats hero;
	private GameObject[] equipped;
	private Item[] equipped_item;
	private string[] ids;
	public Stats stats;
	private InventorySlot[] inventory_slots;
	
	public GameObject inventory_end;
	
	IDictionary<string, GameObject> itemLibrary = new Dictionary<string, GameObject>();
	// Use this for initialization
	void Start () {
        

		inventory = new Inventory();
		for (int i = 0; i < item_tier.Length; i++) 
		{
			foreach (GameObject this_item in item_tier[i].items)
			{
				itemLibrary[this_item.name] = this_item;
			}
		}
		itemLibrary["000000"] = empty_slot;
		num_of_items = -1;
		
		hero = new HeroStats();
		hero.load_data();
		equipped = new GameObject[5];
		equipped_item = new Item[5];
		equipped[0] = GameObject.Find("EquippedHead");
		equipped[1] = GameObject.Find("EquippedWeapon");
		equipped[2] = GameObject.Find("EquippedArmor");
		equipped[3] = GameObject.Find("EquippedCore1");
		equipped[4] = GameObject.Find("EquippedCore2");
		stats = hero.get_current_stats();
		StartEquipped();
		StartInventory();
	}
	
	void StartInventory() {
		inventory.load_inventory();
		num_of_items = inventory.numItems;
		ReloadInventory();
	}
	
	public void UpdateInventory () {
		string[] new_items = new string[0];
		for (int i = 0; i < inventory_slots.Length; i++)
		{
			inventory_slots[i].SetItem(inventory_slots[i].item_id,itemLibrary[inventory_slots[i].item_id],i);
			if (inventory_slots[i].item_id != "000000")
			{
				Array.Resize<string>(ref new_items, new_items.Length+1);
				new_items[new_items.Length-1] = inventory_slots[i].item_id;
			}
		}
		inventory.set_inventory(new_items);
		inventory.store_inventory();
	}
	
	public void ReloadInventory() {
		Debug.Log("Reloading Inventory");
		foreach (Transform child in transform)
		{
			if (child.tag != "Finish")
			GameObject.Destroy(child.gameObject);
		}
		transform.localScale = new Vector3(1f,1f,1f);
		int inv_size;
		if (num_of_items <= 9)
		inv_size = 15;
		else
		inv_size = ((num_of_items/3)+3)*3;
		
		inventory_slots = new InventorySlot[inv_size];
		int x_offset = 0;
		int y_offset = -1;
		GameObject current_slot;
		InventorySlot current_slot_script;
		for (int i = 0; i < inv_size; i++)
		{
			if (i == -1 || i % 3 != 0)
			{
				x_offset++;
			}
			else //if (i != 0 && i % 3 == 0)
			{
				x_offset = 0;
				y_offset++;
			}
			
			Vector3 position = gameObject.transform.position + (Vector3.right*x_offset) + (Vector3.down*y_offset);
			//Now create slots with inventory in them
			current_slot = (GameObject)Instantiate(inventory_slot,position,Quaternion.identity);
			current_slot.transform.parent = gameObject.transform;
			current_slot.transform.localScale = new Vector3(1f,1f,1f);
			current_slot_script = current_slot.GetComponent<InventorySlot>();
			//Place inventory_end at the last slot position for space determining
			inventory_end.transform.position = position + Vector3.down;
			if (i < num_of_items)
			{
				current_slot_script.SetItem(inventory.items[i], itemLibrary[inventory.items[i]], i);
			}
			else
			{
				current_slot_script.SetItem("000000", itemLibrary["000000"], i);
			}
			//Increment offset
			inventory_slots[i] = current_slot_script;
		}
		transform.localScale = new Vector3(1.9f,1.9f,1f);
	}
	
	void StartEquipped() {
		ids = hero.get_equipped_item();
		for (int i = 0; i < 5; i++)
		{
			if (i == 0)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.HEAD);
			else if (i == 1)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.WEAPON);
			else if (i == 2)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.ARMOR);
			else if (i >= 3)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.CORE);
		}
		UpdateEquipped();
	}
	public void UpdateEquipped() {
		stats.item_hp = 0;
		stats.item_armor = 0;
		stats.item_damage = 0;
		stats.item_energy = 0;
		stats.item_penetration = 0;
		stats.item_luck = 0;
		for (int i = 0; i  < 5; i++)
		{
			ids[i] = equipped[i].GetComponent<EquippedSlot>().item_id;
			
			if (i == 0)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.HEAD);
			else if (i == 1)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.WEAPON);
			else if (i == 2)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.ARMOR);
			else if (i >= 3)
			equipped[i].GetComponent<EquippedSlot>().SetItem(ids[i],itemLibrary[ids[i]],Item.ItemType.CORE);
		
			equipped_item[i] = equipped[i].GetComponentInChildren<Item>();
			stats.item_hp += equipped_item[i].hp;
			stats.item_armor += equipped_item[i].armor;
			stats.item_damage += equipped_item[i].damage;
			stats.item_energy += equipped_item[i].energy;
			stats.item_penetration += equipped_item[i].penetration;
			stats.item_luck += equipped_item[i].luck;
			
			stats.equipment[i] = ids[i];
			hero.save_data(stats);
		}
	}
	
	public void InventorySlotClicked(string type) {
		if (type == "H")
		{
			equipped[0].SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
		}
		if (type == "W")
		{
			equipped[1].SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
		}
		if (type == "B")
		{
			equipped[2].SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
		}
		if  (type == "C")
		{
			equipped[3].SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void AddCurrency(int num) {
		inventory.change_currency(num);
		UpdateInventory();
	}
	
	public int ReturnCurrency() {
		return inventory.get_currency();
	}
	
	public int ReturnPaidCurrency() {
		return inventory.get_paid_currency();
	}
}