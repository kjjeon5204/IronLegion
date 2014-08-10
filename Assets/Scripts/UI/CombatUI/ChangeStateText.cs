using UnityEngine;
using System.Collections;

public class ChangeStateText : MonoBehaviour {
    public TextMesh stateText;
    Color originalColor;
    Color warningColor;

    float maxWarningDuration;
    float flashInterval;
    bool flashOn;

    public void change_state_text(string inputText)
    {
        stateText.text = inputText;
        flashOn = true;
        flashInterval = Time.time + 0.3f;
        maxWarningDuration = Time.time + 1.5f;
        stateText.color = warningColor;
    }

	// Use this for initialization
	void Start () {
        originalColor = stateText.color;
        warningColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        stateText.text = "Close";
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time < maxWarningDuration)
        {
            if (Time.time > flashInterval)
            {
                if (stateText.renderer.enabled == true)
                {
                    stateText.renderer.enabled = false;
                    flashInterval = Time.time + 0.2f;
                }
                else
                {
                    stateText.renderer.enabled = true;
                    flashInterval = Time.time + 0.3f;
                }
            }
            if (stateText.renderer.enabled == true && 
                Time.time < flashInterval - 0.2f)
            {
                Color tempColor = stateText.color;
                tempColor.a -= 5.0f * Time.deltaTime;
                if (tempColor.a < 0.0f)
                {
                    tempColor.a = 0.0f;
                }
                stateText.color = tempColor;
            }
            if (stateText.renderer.enabled == true &&
                Time.time > flashInterval - 0.1f)
            {
                Color tempColor = stateText.color;
                tempColor.a += 5.0f * Time.deltaTime;
                if (tempColor.a > 1.0f)
                {
                    tempColor.a = 1.0f;
                }
                stateText.color = tempColor;
            }
        }
        else if (stateText.renderer.enabled == false || stateText.color != originalColor)
        {
            stateText.renderer.enabled = true;
            stateText.color = originalColor;
        }
	}
}
