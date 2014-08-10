using UnityEngine;
using System.Collections;



public class ShockWaveControl : MonoBehaviour {
    public GameObject shockWaveSave;
    public enum ButtonControlType
    {
        REPEATED,
        ONCE
    }

    public ButtonControlType myButtonType;
    float buttonAppearTracker;
    bool shockWaveActive;


    public void activate_button()
    {
        if (myButtonType == ButtonControlType.REPEATED)
        {
            shockWaveActive = true;
            buttonAppearTracker = Time.time + 0.5f;
        }
        if (myButtonType == ButtonControlType.ONCE)
        {
            GameObject shockWave = (GameObject)Instantiate(shockWaveSave,
                        transform.position, transform.rotation);
            shockWave.GetComponent<ButtonShockWave>().initialize_button();
        }
    }

    public void deactivate_button_shock()
    {
        shockWaveActive = false;
    }
	
	// Update is called once per frame
	
    void Update () {
        if (myButtonType == ButtonControlType.REPEATED)
        {
            if (shockWaveActive == true)
            {
                if (Time.time > buttonAppearTracker)
                {
                    buttonAppearTracker = Time.time + 0.5f;
                    GameObject shockWave = (GameObject)Instantiate(shockWaveSave,
                        transform.position, transform.rotation);
                    shockWave.GetComponent<ButtonShockWave>().initialize_button();
                }
            }
        }
	}
}
