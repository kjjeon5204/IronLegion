using UnityEngine;
using System.Collections;

public class YesConfirm : MonoBehaviour {
	public string behavior;
	private ActivateConfirmation active;
	private FirstLogIn data;
	public GameObject loading_screen;
	public GameObject on_screen;
	
	void Awake() {
		behavior = "";
		active = GameObject.Find("Confirm Level").GetComponent<ActivateConfirmation>();
		data = GameObject.Find("DataFileChecker").GetComponent<FirstLogIn>();
	}
	// Use this for initialization
	void Clicked() {
		switch (behavior) {
			case "LEVEL":
				loading_screen.transform.position = on_screen.transform.position;
				active.Reject();
				Application.LoadLevel("BattleScene");
				break;
			case "RESET DATA":
				active.Confirm(null , -1,"RESET DATA CONFIRM");
				break;
			case "RESET DATA CONFIRM":
				data.reset_data();
				loading_screen.transform.position = on_screen.transform.position;
				Application.LoadLevel(Application.loadedLevelName);
				break;
			default:
				break;
		}
	}
}