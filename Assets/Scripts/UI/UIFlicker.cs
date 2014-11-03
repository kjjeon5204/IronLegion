using UnityEngine;
using System.Collections;

public class UIFlicker : MonoBehaviour {
    SpriteRenderer myRenderer;
    public float switchRate;
    bool rendererSwitch = false;
    public Color secondaryColor;
    float switchSpeed;
    float switchTracker;

    float timeTracker;
    Color originalColor;
    Color myLerpedColor;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
        originalColor = myRenderer.color;
        switchSpeed = switchRate;
        myLerpedColor = originalColor;
        switchTracker = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (rendererSwitch == true)
        {
            //Debug.Log(switchTracker + " negative!");
            switchTracker += switchSpeed * Time.deltaTime;
            if (switchTracker >= 1.0f)
            {
                rendererSwitch = false;
                switchTracker = 1.0f;
            }
        }
        else
        {
            //Debug.Log(switchTracker + " positive!");
            switchTracker -= switchSpeed * Time.deltaTime;
            if (switchTracker <= 0.0f)
            {
                rendererSwitch = true;
                switchTracker = 0.0f;
            }
        }
        myLerpedColor = Color.Lerp(originalColor, secondaryColor, switchTracker);
        myRenderer.color = myLerpedColor;
	}
}
