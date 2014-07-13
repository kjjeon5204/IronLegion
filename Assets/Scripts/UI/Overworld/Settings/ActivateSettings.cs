using UnityEngine;
using System.Collections;

public class ActivateSettings : MonoBehaviour {
	private GameObject settings;
	private GameObject waiting;
	private GameObject onScreen;
	
	// Use this for initialization
	void Start () {
		waiting = GameObject.Find("Waiting Area");
		onScreen = GameObject.Find("On Screen");
		settings = GameObject.Find("Settings Sprite");
	}
	
	void Clicked() {
		MoveSettingsOnScreen();
	}
	
	public void MoveSettingsOnScreen() {
		settings.transform.position = onScreen.transform.position;
	}
	
	public void MoveSettingsBack() {
		settings.transform.position = waiting.transform.position;
	}
}
