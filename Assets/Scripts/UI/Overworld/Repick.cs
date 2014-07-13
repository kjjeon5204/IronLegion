using UnityEngine;
using System.Collections;

public class Repick : MonoBehaviour {
	
	void Clicked() {
		ActivateConfirmation reject = GameObject.Find("Confirm Level").GetComponent<ActivateConfirmation>();
		reject.Reject();
	}
}
