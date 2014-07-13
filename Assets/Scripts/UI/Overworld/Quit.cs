using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {

	void Clicked() {
		if (!Application.isEditor)
		{
			Application.Quit();
		}
	}
}
