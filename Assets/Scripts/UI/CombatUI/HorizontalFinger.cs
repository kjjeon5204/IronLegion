using UnityEngine;
using System.Collections;

public class HorizontalFinger : MonoBehaviour {
    public GameObject fingerObject;
    public GameObject rightCornerObject;
    public GameObject leftCornerObject;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fingerObject.transform.Translate(Vector3.right * Time.deltaTime * 2.0f);
        if (fingerObject.transform.position.x > rightCornerObject.transform.position.x)
        {
            fingerObject.transform.position = leftCornerObject.transform.position;
        }
	}
}
