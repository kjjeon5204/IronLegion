using UnityEngine;
using System.Collections;

public class VerticalFinger : MonoBehaviour {
    public GameObject fingerObject;
    public GameObject upperLimit;
    public GameObject lowerLimit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fingerObject.transform.position += Vector3.down * 2.0f * Time.deltaTime;
        if (fingerObject.transform.position.y < lowerLimit.transform.position.y)
        {
            fingerObject.transform.position = upperLimit.transform.position;
        }
	}
}
