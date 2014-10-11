using UnityEngine;
using System.Collections;

public class ScreenFadeIN : MonoBehaviour {

	public float fadeSpeed = 1f;          // Speed that the screen fades to and from black.

private bool fadein =  true;      // Whether or not the scene is still fading in.
private bool fadeout = false;

public GUITexture guiTextre;

 

	void Awake () {
		// Set the texture so that it is the the size of the screen and covers it.
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		fadeout = false;
	}

	void Update () {
		if(fadein)
		StartScene();
		if(fadeout)
		EndSceneFunc();
	}

	public void FadeToClear () {
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	public void FadeToBlack () {
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	void StartScene () {
		FadeToClear();

		if(guiTexture.color.a <= 0.05f)
		{
		guiTexture.color = Color.clear;
		guiTexture.enabled = false;
		fadein = false;
		}
	}
	
	public void EndScene() {
		fadeout = true;
	}

	void EndSceneFunc () {
		guiTexture.enabled = true;
		FadeToBlack();
		if(guiTexture.color.a >= 0.95f)
		{
			guiTexture.color = Color.black;
			fadeout = false;
			//Do whatever you need
		}
	}
	
	public void FadeIn() {
		fadein = true;
		fadeout = false;
	}
	
	public void FadeOut() {
		fadeout = true;
		fadein = false;
	}
	
	
	
	
}