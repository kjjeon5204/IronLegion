using UnityEngine;
using System.Collections;

public class InventorySlot : MonoBehaviour {
	
	public string item_id;
	private Sprite item_image;
	private int index;
	private Item slot_item;
	private GameObject highlight;
	private SpriteRenderer sprite;
	private EquipControls swapping;
	private ItemControls item_controls;
	// Use this for initialization
	void Awake () {
		item_id = "000000";
		highlight = GameObject.Find("InventoryHighlight");
		sprite = GetComponent<SpriteRenderer>();
		swapping = GameObject.Find("SwapButton").GetComponent<EquipControls>();
		item_controls = GameObject.Find("InventoryStart").GetComponent<ItemControls>();
	}
	
	void Update() {
		bool isClicked = false;
		foreach (Transform child in transform)
		{
			if (child.name == "InventoryHighlight")
			{
				isClicked = true;
			}
		}
		if (isClicked)
		{
			sprite.color = Color.green;
		}
		else
		{
			sprite.color = Color.white;
		}
	}
	
	// Update is called once per frame
	void Clicked() {
		highlight.transform.parent = gameObject.transform;
		highlight.transform.position = gameObject.transform.position;
		highlight.transform.localScale = new Vector3(1f,1f,1f);
		swapping.inventory_to_switch = GetComponent<InventorySlot>();
		swapping.item_inventory = slot_item;
		swapping.sprite_inventory = item_image;
		
		Debug.Log(item_id.Substring(2,1));
		item_controls.InventorySlotClicked(item_id.Substring(2,1));
	}
	
	public void SetItem(string id, GameObject item,int ind) {
		foreach (Transform child in transform)
		{
			if (child.name != "InventoryHighlight")
			GameObject.Destroy(child.gameObject);
		}
		item_id = id;
		GameObject current_item = (GameObject)Instantiate(item,transform.position,Quaternion.identity);
		item_image = current_item.GetComponent<SpriteRenderer>().sprite;
		current_item.transform.parent = gameObject.transform;
		current_item.transform.localScale = new Vector3(1f,1f,1f);
		slot_item = current_item.GetComponent<Item>();
		index = ind;
	}
}
