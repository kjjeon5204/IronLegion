using UnityEngine;
using System.Collections;

public class AutoClick : MonoBehaviour {
	public bool dont_click;
	// Use this for initialization
	void Start () {
		if (!dont_click)
		gameObject.SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
	}
}
