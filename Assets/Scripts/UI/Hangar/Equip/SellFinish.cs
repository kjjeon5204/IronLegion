using UnityEngine;
using System.Collections;

public class SellFinish : MonoBehaviour {
	public SellButton selling;

	void Clicked() {
		selling.FinishSell();
	}
}
