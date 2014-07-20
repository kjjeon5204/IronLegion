using UnityEngine;
using System.Collections;

public class CustomHitBox : MonoBehaviour {

    void OnTriggerEnter(Collider hitObject)
    {
        if (hitObject.tag == "Character")
        {

        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
