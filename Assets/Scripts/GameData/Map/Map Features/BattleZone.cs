using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleZone : MapChargeFlag {
	IDictionary<GameObject, MyPathing> pathLibrary = new Dictionary<GameObject, MyPathing>();

	public MyPathing get_path(GameObject targetZone) {
		if (pathLibrary.ContainsKey(targetZone)) {
			Debug.Log ("Path at " + name + " to " + targetZone.name + " found!");
			return pathLibrary[targetZone];
		}
		else {
			Debug.LogError ("Path at " + name + " to " + targetZone.name + " not found!");
			return null;
		}
	}

	// Use this for initialization
	void Start () {
		for (int ctr = 0; ctr < transform.childCount; ctr ++) {
			MyPathing tempHolder = transform.GetChild (ctr).GetComponent<MyPathing>();
			pathLibrary[tempHolder.flagList[tempHolder.flagList.Length - 1]] = 
				tempHolder;
			//Debug.Log ("Battle zone " + name + " path " + tempHolder.gameObject + " initialized!");
		}
	}
}
