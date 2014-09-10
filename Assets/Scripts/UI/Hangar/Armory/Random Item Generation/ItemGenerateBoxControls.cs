﻿using UnityEngine;
using System.Collections;

public class ItemGenerateBoxControls : MonoBehaviour {
    public GameObject lid;
    bool lidControl = false;
    bool initializeFirstActive = false;
    float activationTimeTracker;
    private Quaternion lidOrientation;

    public void activate_box()
    {
        activationTimeTracker = 0.0f;
        lidControl = true;
    }

    void OnEnable() {
        if (initializeFirstActive == false)
        {
            lidOrientation = lid.transform.rotation;
            initializeFirstActive = true;
        }
        lid.transform.rotation = lidOrientation;
    }
    
    void activate_box_process()
    {
        transform.Rotate(Vector3.forward * 30.0f * Time.deltaTime);
        if (activationTimeTracker >= 3.0f)
        {
            lidControl = false;
        }
        activationTimeTracker += Time.deltaTime;
    }

    

	// Use this for initialization
	void Start () {
        lidOrientation = lid.transform.rotation;
        initializeFirstActive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (lidControl == true)
        {
            activate_box_process();
        }
	}
}