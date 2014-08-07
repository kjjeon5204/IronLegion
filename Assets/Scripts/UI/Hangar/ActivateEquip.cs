using UnityEngine;
using System.Collections;

public class ActivateEquip : MonoBehaviour {
	public GameObject equip;
	
	public HangarControls hangar;
	private GameObject border_bot;
	private GameObject border_top;
	
	public GameObject scroll_top;
	public GameObject scroll_bot;
	public Camera second_cam;
	
	void Clicked () {
		hangar.MoveOffScreen();
		hangar.SetObjectOnScreen(equip);
		
		border_bot = GameObject.Find("BorderBotEquip");
		border_top = GameObject.Find("BorderTopEquip");
		
		Vector3 bottom_left = Camera.main.WorldToViewportPoint(border_bot.transform.position);
		Vector3 top_right = Camera.main.WorldToViewportPoint(border_top.transform.position);
		
		second_cam.rect = new Rect(bottom_left.x, bottom_left.y,top_right.x-bottom_left.x,top_right.y-bottom_left.y);
		scroll_top.transform.position = second_cam.ViewportToWorldPoint(new Vector3(0f,1f,10f));
		scroll_bot.transform.position = second_cam.ViewportToWorldPoint(new Vector3(0f,0f,10f));
	}
}
