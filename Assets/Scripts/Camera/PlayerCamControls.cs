using UnityEngine;
using System.Collections;

public struct CamControlOption
{
    public bool camEffectActive;
    public float effectDuration;
}

public class PlayerCamControls : MonoBehaviour {
    protected CamControlOption cameraShakeOptionType1= new CamControlOption();
    float xValCamOffset = 0.07f;
    float timeTracker;

    public void cam_control_activate(string inputCommand, float inDuration)
    {
        if (inputCommand == "LEFT_RIGHT_SHAKE")
        {
            cameraShakeOptionType1.camEffectActive = true;
            cameraShakeOptionType1.effectDuration = inDuration;
            timeTracker = Time.time;
            activate_cam_control(this.cameraShakeOptionType1, inDuration);
        }
    }

    void activate_cam_control(CamControlOption inputCamEffect, float inDuration) 
    {
        Debug.Log("Camera effect active!");
        inputCamEffect.camEffectActive = true;
        inputCamEffect.effectDuration = inDuration;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraShakeOptionType1.camEffectActive == true)
        {
            if (xValCamOffset > 0.0f && timeTracker < Time.time)
            {
                timeTracker = Time.time + 0.005f;
                transform.Translate(Vector3.right * xValCamOffset);
                xValCamOffset *= -1.0f;
            }
            if (xValCamOffset < 0.0f && timeTracker < Time.time)
            {
                timeTracker = Time.time + 0.005f;
                transform.Translate(Vector3.right * xValCamOffset);
                xValCamOffset *= -1.0f;
            }
            cameraShakeOptionType1.effectDuration -= Time.deltaTime;
            if (cameraShakeOptionType1.effectDuration < 0.0f)
            {

                cameraShakeOptionType1.camEffectActive = false;
            }
        }
	}
}
