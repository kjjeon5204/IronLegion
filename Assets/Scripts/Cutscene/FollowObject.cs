using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {
	public GameObject follow;
	private bool start;
	
	void Update () {
		if (start)
		{
			transform.LookAt(follow.transform,Vector3.up);
		}
	}
	
	void Follow() {
		start = true;
	}
}
