using UnityEngine;
using System.Collections;

public class ItemGenerateBoxControls : MonoBehaviour {
    public GameObject lid;
    bool lidControl = false;
    bool initializeFirstActive = false;
    public GameObject boxLight;
    float activationTimeTracker;
    private Quaternion lidOrientation;
    public GameObject lightParticle;

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
        boxLight.SetActive(false);
    }
    
    void activate_box_process()
    {
        if (activationTimeTracker < 1.0f)
            lid.transform.Rotate(Vector3.left * 90.0f * Time.deltaTime);
        else
        {
            if (boxLight.activeInHierarchy == false)
                boxLight.SetActive(true);
        }
        if (activationTimeTracker > 0.5f)
        {
            if (lightParticle.activeInHierarchy == false)
                lightParticle.SetActive(true);
        }
        if (activationTimeTracker >= 1.2f)
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
	public bool run_lid() {
        if (lidControl == true)
        {
            activate_box_process();
        }
        return lidControl;
	}
}
