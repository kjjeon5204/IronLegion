using UnityEngine;
using System.Collections;

public class LowEnergyWarning : MonoBehaviour {
    float timer;

    void OnEnable()
    {
        timer = Time.time + 2.0f;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timer)
        {
            gameObject.SetActive(false);
        }
	}
}
