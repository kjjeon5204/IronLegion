using UnityEngine;
using System.Collections;

public class ActivateAbilities : MonoBehaviour {
	public GameObject abilities;
	
	public HangarControls hangar;
	
	public GameObject border_bot;
	public GameObject border_top;
	public Camera second_cam;
	
	void Clicked () {
		hangar.MoveOffScreen();
		hangar.SetObjectOnScreen(abilities);
		
		Vector3 bottom_left = Camera.main.WorldToViewportPoint(border_bot.transform.position);
		Vector3 top_right = Camera.main.WorldToViewportPoint(border_top.transform.position);
		
		second_cam.rect = new Rect(bottom_left.x, bottom_left.y,top_right.x-bottom_left.x,top_right.y-bottom_left.y);
	}
}