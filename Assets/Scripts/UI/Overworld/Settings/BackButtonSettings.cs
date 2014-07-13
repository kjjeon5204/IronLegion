using UnityEngine;
using System.Collections;

public class BackButtonSettings : MonoBehaviour {

	private ActivateSettings controller;
	
	void Start () {
		controller = GameObject.Find("Settings").GetComponent<ActivateSettings>();
	}
	
	void Clicked() {
		controller.MoveSettingsBack();
	}
}
