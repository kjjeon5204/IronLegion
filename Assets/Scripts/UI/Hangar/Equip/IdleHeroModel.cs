using UnityEngine;
using System.Collections;

public class IdleHeroModel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		animation.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime*20f);
	}
}
