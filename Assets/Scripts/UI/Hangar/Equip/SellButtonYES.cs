using UnityEngine;
using System.Collections;

public class SellButtonYES : MonoBehaviour {
	public SellButton button;

	void Clicked() {
		button.SellItem();
	}
}
