using UnityEngine;
using System.Collections;

public class SellButton : MonoBehaviour {
	public EquipControls swapping;
	public GameObject confirmation;
	public GameObject onScreen;
	public GameObject waiting;
	public TextMesh name;
	public SpriteRenderer icon;
	
	void Clicked () {
		if (swapping.CheckSellable())
		{
			confirmation.transform.position = onScreen.transform.position;
			icon.sprite = swapping.sprite_inventory;
			name.text = swapping.item_inventory.itemName+"?";
		}
	}
	
	public void SellItem() {
		swapping.SellSelectedItem();
		StopSelling();
	}
	
	public void StopSelling() {
		confirmation.transform.position = waiting.transform.position;
	}
}
