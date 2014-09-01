using UnityEngine;
using System.Collections;

public class ActivateArmory : MonoBehaviour {
	private GameObject armory;
	
	private HangarControls hangar;
	private Camera second_cam;

	void Start () {
		armory = GameObject.Find("ArmoryFrame");
		hangar = GameObject.Find("UI").GetComponent<HangarControls>();
	}
	
	void Clicked () {
		hangar.MoveOffScreen();
		hangar.SetObjectOnScreen(armory);
		
		second_cam = GameObject.Find("Camera").GetComponent<Camera>();
		second_cam.rect = new Rect(0,0,1f,1f);
        armory.SetActive(false);
        armory.SetActive(true);
	}
}
