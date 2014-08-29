using UnityEngine;
using System.Collections;

public class MechDetonator : MonoBehaviour {
	public GameObject myObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Instantiate (myObject, transform.position, Quaternion.identity); 
	}
}
