using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetInventory : MonoBehaviour {
	public GameObject[] items;
	public GameObject inventory_slot;
	private Inventory inventory;
	public int num_of_items;
	public GameObject empty_slot;
	
	//private HeroStats hero;
	private GameObject[] equipped;
	
	IDictionary<string, GameObject> itemLibrary = new Dictionary<string, GameObject>();
	// Use this for initialization
	void Start () {
		inventory = new Inventory();
		foreach (GameObject this_item in items) 
		{
			itemLibrary[this_item.name] = this_item;
		}
		num_of_items = -1;
		
		//hero = new HeroStats();
		//hero.load_data();
		equipped = new GameObject[5];
		equipped[0] = GameObject.Find("EquippedHead");
		equipped[1] = GameObject.Find("EquippedWeapon");
		equipped[2] = GameObject.Find("EquippedArmor");
		equipped[3] = GameObject.Find("EquippedCore1");
		equipped[4] = GameObject.Find("EquippedCore2");
	}
	
	void Update() {
		UpdateInventory();
	}
	
	public void UpdateInventory () {
		inventory.load_inventory();
		int temp = inventory.numItems;
		if (temp != num_of_items)
		{
			num_of_items = temp;
			ReloadInventory();
		}
	}
	
	void ReloadInventory() {
	Debug.Log("Reloading Inventory");
		foreach (Transform child in transform)
		{
			if (child.tag != "Finish")
			GameObject.Destroy(child.gameObject);
		}
		transform.localScale = new Vector3(1f,1f,1f);
		int inv_size;
		if (num_of_items <= 15)
		inv_size = 15;
		else
		inv_size = ((num_of_items/3)+2)*3;
		
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
			if (i < num_of_items)
			{
				current_slot_script.SetItem(inventory.items[i], itemLibrary[inventory.items[i]], i);
			}
			else
			{
				current_slot_script.SetItem("000000", empty_slot, -1);
			}
			//Increment offset
			
		}
		transform.localScale = new Vector3(1.9f,1.9f,1f);
	}
	
	public void UpdateEquipped() {
		
	}
	
	public void AddItem(string id)
	{
		inventory.add_item(id);
		inventory.store_inventory();
	}
}
