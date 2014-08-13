using UnityEngine;
using System.Collections;

public class SellButtonNO : MonoBehaviour {
	public SellButton button;
	// Use this for initialization
	void Clicked() {
		button.StopSelling();
	}
}
