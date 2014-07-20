using UnityEngine;
using System.Collections;

public class EquipControls : MonoBehaviour {
	
	public InventorySlot inventory_to_switch;
	public EquippedSlot equipped_to_switch;
	
	public Item item_inventory;
	public Item item_equipped;
	
	public Sprite sprite_inventory;
	public Sprite sprite_equipped;
	
	public bool swappable;
	private ItemControls inventory;
	
	private GameObject inventory_description;
	private GameObject equipped_description;
	
	private TextMesh[] inventory_description_text;
	private TextMesh[] equipped_description_text;
	
	SpriteRenderer image;
	// Use this for initialization
	void Awake () {
		image = GetComponent<SpriteRenderer>();
		inventory = GameObject.Find("InventoryStart").GetComponent<ItemControls>();
		
		inventory_description = GameObject.Find("InventoryItemDescription");
		equipped_description = GameObject.Find("EquippedItemDescription");
		
		inventory_description_text = new TextMesh[7];
		equipped_description_text = new TextMesh[7];
		
		inventory_description_text[0] = inventory_description.transform.Find("Name").GetComponent<TextMesh>();
		inventory_description_text[1] = inventory_description.transform.Find("HP").GetComponent<TextMesh>();
		inventory_description_text[2] = inventory_description.transform.Find("Armor").GetComponent<TextMesh>();
		inventory_description_text[3] = inventory_description.transform.Find("Damage").GetComponent<TextMesh>();
		inventory_description_text[4] = inventory_description.transform.Find("Energy").GetComponent<TextMesh>();
		inventory_description_text[5] = inventory_description.transform.Find("Penetration").GetComponent<TextMesh>();
		inventory_description_text[6] = inventory_description.transform.Find("Luck").GetComponent<TextMesh>();
		
		equipped_description_text[0] = equipped_description.transform.Find("Name").GetComponent<TextMesh>();
		equipped_description_text[1] = equipped_description.transform.Find("HP").GetComponent<TextMesh>();
		equipped_description_text[2] = equipped_description.transform.Find("Armor").GetComponent<TextMesh>();
		equipped_description_text[3] = equipped_description.transform.Find("Damage").GetComponent<TextMesh>();
		equipped_description_text[4] = equipped_description.transform.Find("Energy").GetComponent<TextMesh>();
		equipped_description_text[5] = equipped_description.transform.Find("Penetration").GetComponent<TextMesh>();
		equipped_description_text[6] = equipped_description.transform.Find("Luck").GetComponent<TextMesh>();
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
		UpdateDescriptions();
	}
	
	void UpdateDescriptions() {
		if (item_inventory != null)
		{
		
			inventory_description_text[0].text = item_inventory.itemName;
			
			if (item_inventory.hp < 0)
			inventory_description_text[1].text = "HP: -" + item_inventory.hp;
			else if (item_inventory.hp > 0)
			inventory_description_text[1].text = "HP: +" + item_inventory.hp;
			else
			inventory_description_text[1].text = "";
			
			if (item_inventory.armor < 0)
			inventory_description_text[2].text = "Armor: -" + item_inventory.armor;
			else if (item_inventory.armor > 0)
			inventory_description_text[2].text = "Armor: +" + item_inventory.armor;
			else
			inventory_description_text[2].text = "";
			
			if (item_inventory.damage < 0)
			inventory_description_text[3].text = "Damage: -"+item_inventory.damage;
			else if (item_inventory.damage > 0)
			inventory_description_text[3].text = "Damage: +"+item_inventory.damage;
			else
			inventory_description_text[3].text = "";
			
			if (item_inventory.energy < 0)
			inventory_description_text[4].text = "Energy: -"+item_inventory.energy;
			else if (item_inventory.energy > 0)
			inventory_description_text[4].text = "Energy: +"+item_inventory.energy;
			else
			inventory_description_text[4].text = "";
			
			if (item_inventory.penetration < 0)
			inventory_description_text[5].text = "Penetration: -" + item_inventory.penetration;
			else if (item_inventory.penetration > 0)
			inventory_description_text[5].text = "Penetration: +" + item_inventory.penetration;
			else
			inventory_description_text[5].text = "";
			
			if (item_inventory.luck < 0)
			inventory_description_text[6].text = "Luck: -" + item_inventory.luck;
			else if (item_inventory.luck > 0)
			inventory_description_text[6].text = "Luck: +" + item_inventory.luck;
			else
			inventory_description_text[6].text = "";
		}
		if (item_equipped != null)
		{
			equipped_description_text[0].text = item_equipped.itemName;
			
			if (item_equipped.hp < 0)
			equipped_description_text[1].text = "HP: -" + item_equipped.hp;
			else if (item_equipped.hp > 0)
			equipped_description_text[1].text = "HP: +" + item_equipped.hp;
			else
			equipped_description_text[1].text = "";
			
			if (item_equipped.armor < 0)
			equipped_description_text[2].text = "Armor: -" + item_equipped.armor;
			else if (item_equipped.armor > 0)
			equipped_description_text[2].text = "Armor: +" + item_equipped.armor;
			else
			equipped_description_text[2].text = "";
			
			if (item_equipped.damage < 0)
			equipped_description_text[3].text = "Damage: -"+item_equipped.damage;
			else if (item_equipped.damage > 0)
			equipped_description_text[3].text = "Damage: +"+item_equipped.damage;
			else
			equipped_description_text[3].text = "";
			
			if (item_equipped.energy < 0)
			equipped_description_text[4].text = "Energy: -"+item_equipped.energy;
			else if (item_equipped.energy > 0)
			equipped_description_text[4].text = "Energy: +"+item_equipped.energy;
			else
			equipped_description_text[4].text = "";
			
			if (item_equipped.penetration < 0)
			equipped_description_text[5].text = "Penetration: -" + item_equipped.penetration;
			else if (item_equipped.penetration > 0)
			equipped_description_text[5].text = "Penetration: +" + item_equipped.penetration;
			else
			equipped_description_text[5].text = "";
			
			if (item_equipped.luck < 0)
			equipped_description_text[6].text = "Luck: -" + item_equipped.luck;
			else if (item_equipped.luck > 0)
			equipped_description_text[6].text = "Luck: +" + item_equipped.luck;
			else
			equipped_description_text[6].text = "";
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
			
			Sprite temp_sprite = sprite_equipped;
			sprite_equipped = sprite_inventory;
			sprite_inventory = temp_sprite;
			
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
