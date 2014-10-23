using UnityEngine;
using System.Collections;

public class ExitToOverworld : MonoBehaviour {

	// Use this for initialization
	void Clicked() {
		Application.LoadLevel("Overworld");
	}
}
