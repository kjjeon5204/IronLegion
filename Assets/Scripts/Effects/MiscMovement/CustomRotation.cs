using UnityEngine;
using System.Collections;

public class CustomRotation : MonoBehaviour {
    public Vector3 rotationVector;
    public float rotationSpeed;

    public Vector3 oscillationVector;
    public float oscilationTime;
    public float amplitude;
    float initialYPos;
    float angleTracker;

    void Start()
    {
        initialYPos = transform.position.y;
    }

	
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);

        if (oscilationTime != 0)
            angleTracker += (Mathf.PI * 2.0f) / oscilationTime * Time.deltaTime;
        if (angleTracker > Mathf.PI * 2.0f)
            angleTracker -= Mathf.PI * 2.0f;
        transform.position = new Vector3(transform.position.x, initialYPos + amplitude * Mathf.Sin(angleTracker),
            transform.position.z);
	}
}
