using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour {
    public SpriteRenderer blackScreenSprite;

    int fadePhase = 0;
    bool fadeActive = false;
    Fade_Black_Intermission intermissionHandler;


    public delegate void Fade_Black_Intermission();



    public void screen_fade_active(Fade_Black_Intermission myHandle)
    {
        fadePhase = 0;
        fadeActive = true;
        intermissionHandler = myHandle;
    }

    public void screen_fade_active()
    {
        fadePhase = 0;
        fadeActive = true;
    }

    public bool screen_fade_is_active()
    {
        return fadeActive;
    }

	
	// Update is called once per frame
	void Update () {
        if (fadeActive == true) {
            if (fadePhase == 0)
            {
                Color tempColor = blackScreenSprite.color;
                tempColor.a += 2.0f * Time.deltaTime;
                if (tempColor.a >= 1.0f)
                {
                    tempColor.a = 1.0f;
                    fadePhase++;
                    intermissionHandler();
                }
                blackScreenSprite.color = tempColor;
            }
            else if (fadePhase == 1)
            {
                Color tempColor = blackScreenSprite.color;
                tempColor.a -= 2.0f * Time.deltaTime;
                if (tempColor.a <= 0.0f)
                {
                    tempColor.a = 0.0f;
                    fadeActive = false;
                }
                blackScreenSprite.color = tempColor;
            }
        }
	}
}
