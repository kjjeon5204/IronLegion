﻿using UnityEngine;
using System.Collections;

public class SetSecondaryCamera : MonoBehaviour {
	private GameObject border_bot;
	private GameObject border_top;
	private Camera second_cam;
	
	void Start () {
		border_bot = GameObject.Find("BorderBotAbility");
		border_top = GameObject.Find("BorderTopAbility");
		second_cam = GameObject.Find("Camera").GetComponent<Camera>();
		
		Vector3 bottom_left = Camera.main.WorldToViewportPoint(border_bot.transform.position);
		Vector3 top_right = Camera.main.WorldToViewportPoint(border_top.transform.position);
		
		second_cam.rect = new Rect(bottom_left.x, bottom_left.y,top_right.x-bottom_left.x,top_right.y-bottom_left.y);
	}
}
