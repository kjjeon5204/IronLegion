using UnityEngine;
using System.Collections;

public class TargetingIndicator : MonoBehaviour {
    public GameObject innerCircle;
    public float changeRate = 1.0f;
    public float rotRate = 90.0f;

    Vector3 initialScale;

    Vector3 targetScale;

    public void initialize_indicator()
    {
        transform.localScale = initialScale;
    }

	// Use this for initialization
	void Start () {
        initialScale = transform.localScale;
        targetScale = initialScale * 0.25f;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale.x > targetScale.x)
        {
            transform.localScale -= changeRate * (new Vector3(1.0f, 1.0f, 0.0f)) * Time.deltaTime;
            if (innerCircle.activeInHierarchy == true)
                innerCircle.SetActive(false);
        }
        else
        {
            if (innerCircle.activeInHierarchy == false)
                innerCircle.SetActive(true);
            innerCircle.transform.Rotate(Vector3.forward, rotRate * Time.deltaTime); 
        }
	}
}
