﻿using UnityEngine;
using System.Collections;

public class ActivateArmory : MonoBehaviour {
	public GameObject armory;
	
	private HangarControls hangar;
	private Camera second_cam;
    public GameObject equipPage;

	void Start () {
		//armory = GameObject.Find("ArmoryFrame");
		hangar = GameObject.Find("UI").GetComponent<HangarControls>();
	}
	
	void Clicked () {
		hangar.MoveOffScreen();
		hangar.SetObjectOnScreen(armory);
        equipPage.SetActive(false);
		
		second_cam = GameObject.Find("Camera").GetComponent<Camera>();
		second_cam.rect = new Rect(0,0,1f,1f);
        armory.SetActive(false);
        armory.SetActive(true);
	}
}
