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
	public DisplayCurrentStats current_stats;
	
	private GameObject inventory_description;
	private GameObject equipped_description;
	
	private TextMesh[] inventory_description_text;
	private TextMesh[] equipped_description_text;
	private SpriteRenderer inventory_description_image;
	private SpriteRenderer equipped_description_image;
	
	public GameObject highlight_item;
	public GameObject highlight_equip;
	public GameObject waiting;
    public PlayerMasterData playerMasterData;

	private string color_start = "<color=lime>";
	private string color_end = "</color>";
	
	
	
	SpriteRenderer image;
	// Use this for initialization
	void Awake () {
		image = GetComponent<SpriteRenderer>();
		inventory = GameObject.Find("InventoryStart").GetComponent<ItemControls>();
		
		inventory_description = GameObject.Find("InventoryItemDescription");
		equipped_description = GameObject.Find("EquippedItemDescription");
		
		inventory_description_image = inventory_description.transform.Find("Icon").GetComponent<SpriteRenderer>();
		equipped_description_image = equipped_description.transform.Find("Icon").GetComponent<SpriteRenderer>();
		
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
	
	void Start() {
		ReloadDescription();
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
			inventory_description_image.sprite = sprite_inventory;
			inventory_description_text[0].text = item_inventory.itemName;
			
			if (item_inventory.hp < 0)
			inventory_description_text[1].text = color_start+"HP: -"+color_end + item_inventory.hp;
			else if (item_inventory.hp > 0)
			inventory_description_text[1].text = color_start+"HP: +"+color_end + item_inventory.hp;
			else
			inventory_description_text[1].text = "";
			
			if (item_inventory.armor < 0)
			inventory_description_text[2].text = color_start+"Armor: -"+color_end + item_inventory.armor;
			else if (item_inventory.armor > 0)
			inventory_description_text[2].text = color_start+"Armor: +"+color_end + item_inventory.armor;
			else
			inventory_description_text[2].text = "";
			
			if (item_inventory.damage < 0)
			inventory_description_text[3].text = color_start+"Damage: -"+color_end+item_inventory.damage;
			else if (item_inventory.damage > 0)
			inventory_description_text[3].text = color_start+"Damage: +"+color_end+item_inventory.damage;
			else
			inventory_description_text[3].text = "";
			
			if (item_inventory.energy < 0)
			inventory_description_text[4].text = color_start+"Energy: -"+color_end+item_inventory.energy;
			else if (item_inventory.energy > 0)
			inventory_description_text[4].text = color_start+"Energy: +"+color_end+item_inventory.energy;
			else
			inventory_description_text[4].text = "";
			
			if (item_inventory.penetration < 0)
			inventory_description_text[5].text = color_start+"Penetration: -"+color_end + item_inventory.penetration;
			else if (item_inventory.penetration > 0)
			inventory_description_text[5].text = color_start+"Penetration: +"+color_end + item_inventory.penetration;
			else
			inventory_description_text[5].text = "";
			
			if (item_inventory.luck < 0)
			inventory_description_text[6].text = color_start+"Luck: -"+color_end + item_inventory.luck;
			else if (item_inventory.luck > 0)
			inventory_description_text[6].text = color_start+"Luck: +"+color_end + item_inventory.luck;
			else
			inventory_description_text[6].text = "";
		}
		if (item_equipped != null)
		{
			equipped_description_image.sprite = sprite_equipped;
			equipped_description_text[0].text = item_equipped.itemName;
			
			if (item_equipped.hp < 0)
			equipped_description_text[1].text = color_start+"HP: -"+color_end + item_equipped.hp;
			else if (item_equipped.hp > 0)
			equipped_description_text[1].text = color_start+"HP: +"+color_end + item_equipped.hp;
			else
			equipped_description_text[1].text = "";
			
			if (item_equipped.armor < 0)
			equipped_description_text[2].text = color_start+"Armor: -"+color_end + item_equipped.armor;
			else if (item_equipped.armor > 0)
			equipped_description_text[2].text = color_start+"Armor: +"+color_end + item_equipped.armor;
			else
			equipped_description_text[2].text = "";
			
			if (item_equipped.damage < 0)
			equipped_description_text[3].text = color_start+"Damage: -"+color_end+item_equipped.damage;
			else if (item_equipped.damage > 0)
			equipped_description_text[3].text = color_start+"Damage: +"+color_end+item_equipped.damage;
			else
			equipped_description_text[3].text = "";
			
			if (item_equipped.energy < 0)
			equipped_description_text[4].text = color_start+"Energy: -"+color_end+item_equipped.energy;
			else if (item_equipped.energy > 0)
			equipped_description_text[4].text = color_start+"Energy: +"+color_end+item_equipped.energy;
			else
			equipped_description_text[4].text = "";
			
			if (item_equipped.penetration < 0)
			equipped_description_text[5].text = color_start+"Penetration: -"+color_end + item_equipped.penetration;
			else if (item_equipped.penetration > 0)
			equipped_description_text[5].text = color_start+"Penetration: +"+color_end + item_equipped.penetration;
			else
			equipped_description_text[5].text = "";
			
			if (item_equipped.luck < 0)
			equipped_description_text[6].text = color_start+"Luck: -"+color_end + item_equipped.luck;
			else if (item_equipped.luck > 0)
			equipped_description_text[6].text = color_start+"Luck: +"+color_end + item_equipped.luck;
			else
			equipped_description_text[6].text = "";
		}
	}
	
	public void ReloadDescription() {
		inventory_description_image.sprite = null;
		inventory_description_text[0].text = "";
		inventory_description_text[1].text = "";
		inventory_description_text[2].text = "Select a slot";
		inventory_description_text[3].text = "to switch";
		inventory_description_text[4].text = "between!";
		inventory_description_text[5].text = "";
		inventory_description_text[6].text = "";
		
		equipped_description_image.sprite = null;
		equipped_description_text[0].text = "";
		equipped_description_text[1].text = "";
		equipped_description_text[2].text = "Select a slot";
		equipped_description_text[3].text = "to switch";
		equipped_description_text[4].text = "between!";
		equipped_description_text[5].text = "";
		equipped_description_text[6].text = "";
		
		highlight_item.transform.position = waiting.transform.position;
		highlight_item.transform.parent = null;
		highlight_equip.transform.position = waiting.transform.position;
		highlight_equip.transform.parent = null;
		swappable = false;
	}
	
	void Clicked() {
		if (swappable)
		{
			string temp;
			temp = inventory_to_switch.item_id;
			inventory_to_switch.item_id = equipped_to_switch.item_id;
			equipped_to_switch.item_id = temp;
            Debug.Log("Newly Equipped: " + temp);
			Item item_temp = new Item();
			item_temp = item_inventory;
			item_inventory = item_equipped;
			item_equipped = item_temp;
			
			Sprite temp_sprite = sprite_equipped;
			sprite_equipped = sprite_inventory;
			sprite_inventory = temp_sprite;
			
			inventory.UpdateInventory();
			inventory.UpdateEquipped();

            
			swappable = false;
			
			highlight_item.transform.position = waiting.transform.position;
			highlight_item.transform.parent = null;
			highlight_equip.transform.position = waiting.transform.position;
			highlight_equip.transform.parent = null;
            current_stats.UpdateStats();
			ReloadDescription();
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
	
	public bool CheckSellable() {
		if (item_inventory != null && item_inventory.itemID != "000000")
		return true;
		
		return false;
	}
	
	public void SellSelectedItem() {
        inventory_to_switch.destroy_holding_item();
        inventory.UpdateInventory();
        Debug.Log("Sell amount: " + item_inventory.sell_price);
        playerMasterData.add_currency(item_inventory.sell_price);
        ReloadDescription();
	}
}
