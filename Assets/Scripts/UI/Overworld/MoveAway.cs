using UnityEngine;
using System.Collections;

public class MoveAway : MonoBehaviour {
	public GameObject place_to_move_to;

	void Clicked() {
		gameObject.transform.position = place_to_move_to.transform.position;
	}
}
