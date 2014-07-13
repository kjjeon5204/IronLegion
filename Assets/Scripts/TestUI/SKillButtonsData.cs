using UnityEngine;
using System.Collections;

public class SKillButtonsData : MonoBehaviour {
    public float curCD;
    public float maxCD;
    public string buttonID;
    bool isActive;

    public string button_pressed()
    {
        //Debug.Log("Button " + gameObject.name + " pressed!");
        curCD = maxCD;
        return buttonID;
    }

    public bool is_on_CD()
    {
        if (curCD > 0.0f) 
        {
            return true;
        }
        Debug.Log("CurrentCD is " + curCD);
        return false;
    }

    public void setup_button(float maxCDInput, float curCDInput)
    {
        maxCD = maxCDInput;
        curCD = curCDInput;
    }

    public bool is_active()
    {
        return isActive;
    }

    public void set_active()
    {
        isActive = true;
    }

    public void set_inactive()
    {
        isActive = false;
    }

	// Use this for initialization
	void Start () {
        isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (curCD > 0.0f)
        {
            curCD -= Time.deltaTime;
        }
        if (curCD < 0.0f)
        {
            curCD = 0.0f;
        }
	}
}
