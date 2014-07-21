using UnityEngine;
using System.Collections;

public class EquippedSlot : MonoBehaviour {
	public string item_id;
	private Sprite item_image;
	private Item slot_item;
	private GameObject highlight;
	private SpriteRenderer sprite;
	private EquipControls swapping;
	// Use this for initialization
	void Awake () {
		highlight = GameObject.Find("EquipHighlight");
		sprite = GetComponent<SpriteRenderer>();
		swapping = GameObject.Find("SwapButton").GetComponent<EquipControls>();
	}
	
	// Update is called once per frame
	void Update () {
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
	
	void Clicked() {
		highlight.transform.parent = gameObject.transform;
		highlight.transform.position = gameObject.transform.position;
		highlight.transform.localScale = new Vector3(1f,1f,1f);
		swapping.equipped_to_switch = GetComponent<EquippedSlot>();
		swapping.item_equipped = slot_item;
		swapping.sprite_equipped = item_image;
	}
	
	public void SetItem(string id, GameObject item,Item.ItemType type) {
		foreach (Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}
		item_id = id;
		GameObject current_item = (GameObject)Instantiate(item,transform.position,Quaternion.identity);
		item_image = current_item.GetComponent<SpriteRenderer>().sprite;
		current_item.transform.parent = gameObject.transform;
		current_item.transform.localScale = new Vector3(1f,1f,1f);
		slot_item = current_item.GetComponent<Item>();
		current_item.GetComponent<Item>().itemType = type;
	}
	
	public Item ReturnItemStats() {
		return slot_item;
	}
}
