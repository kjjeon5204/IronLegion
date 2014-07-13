using UnityEngine;
using System.Collections;

public class ResetButtonSettings : MonoBehaviour {

	private ActivateConfirmation active;

	void Awake() {
		active = GameObject.Find("Confirm Level").GetComponent<ActivateConfirmation>();
	}


	void Clicked() {
		active.Confirm( null, -1,"RESET DATA");
	}
}
