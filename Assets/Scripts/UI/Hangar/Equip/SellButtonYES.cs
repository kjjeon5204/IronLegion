using UnityEngine;
using System.Collections;

public class SellButtonYES : MonoBehaviour {
	public SellButton button;

	void Clicked() {
        Debug.Log("Button Clicked!");
		button.ConfirmSell();
	}
}
