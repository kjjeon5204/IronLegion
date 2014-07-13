using UnityEngine;
using System.Collections;

public class ActivateAllies : MonoBehaviour {
	private GameObject ally;

	private HangarControls hangar;
	private Camera second_cam;
	
	void Start () {
		ally = GameObject.Find("AllyFrame");
		hangar = GameObject.Find("UI").GetComponent<HangarControls>();
	}

	void Clicked () {
		hangar.MoveOffScreen();
		hangar.SetObjectOnScreen(ally);
		
		second_cam = GameObject.Find("Camera").GetComponent<Camera>();
		second_cam.rect = new Rect(0,0,1f,1f);
	}
}
