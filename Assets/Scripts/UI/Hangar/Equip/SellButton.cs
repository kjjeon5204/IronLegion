using UnityEngine;
using System.Collections;

public class SellButton : MonoBehaviour {
	public EquipControls swapping;
	public GameObject confirmation;
	public GameObject display;
	public GameObject onScreen;
	public GameObject waiting;
	public TextMesh name;
	public TextMesh credits;
	public TextMesh total_credits;
	public SpriteRenderer icon;
	public DisplayCurrentStats current_stats;
    public GameObject sellCam;
    public GameObject equipBlinder;
	
	private string start_color = "<color=#ccfcff>";
	private string end_color = "</color>";
	
	void Clicked () {
		if (swapping.CheckSellable())
		{
			confirmation.transform.position = onScreen.transform.position;
			icon.sprite = swapping.sprite_inventory;
			name.text = start_color+swapping.item_inventory.itemName+end_color+" for";
			credits.text = swapping.item_inventory.sell_price + " credits?";
            sellCam.SetActive(true);
            equipBlinder.SetActive(true);
		}
	}
	
	public void SellItem() {
		swapping.SellSelectedItem();
		StopSelling();
	}
	
	public void StopSelling() {
		confirmation.transform.position = waiting.transform.position;
        sellCam.SetActive(false);
        equipBlinder.SetActive(false);
	}
	
	public void ConfirmSell() {
		SellItem();
		display.transform.position = onScreen.transform.position;
		total_credits.text = "You now have "+current_stats.GetCurrency()+" credits";
        sellCam.SetActive(false);
        equipBlinder.SetActive(false);
    }
	
	public void FinishSell() {
		display.transform.position = waiting.transform.position;
	}
}
