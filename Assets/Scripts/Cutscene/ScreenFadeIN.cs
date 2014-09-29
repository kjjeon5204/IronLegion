using UnityEngine;
using System.Collections;

public class ScreenFadeIN : MonoBehaviour {

	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.

private bool sceneStarting = true;      // Whether or not the scene is still fading in.
private bool sceneEnding = false;

public GUITexture guiTextre;

 

	void Awake () {
		// Set the texture so that it is the the size of the screen and covers it.
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		sceneEnding = false;
	}

	void Update () {
		if(sceneStarting)
		StartScene();
		if(sceneEnding)
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
		sceneStarting = false;
		}
	}
	
	public void EndScene() {
		sceneEnding = true;
	}

	void EndSceneFunc () {
		guiTexture.enabled = true;
		FadeToBlack();
		if(guiTexture.color.a >= 0.95f)
		{
			guiTexture.color = Color.black;
			sceneEnding = false;
			//Do whatever you need
		}
	}
}