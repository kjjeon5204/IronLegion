using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {
	public GUISkin TutorialSkin;
	public Texture2D borderLeft;
	public Texture2D borderRight;

	public Texture2D overworldnavButton;
	public Texture2D overworldnavButtonPressed;
	public Texture2D overworldnavBox;
	
	public Texture2D combatButton;
	public Texture2D combatButtonPressed;
	public Texture2D combatBox;
	
	public Texture2D abilitiesButton;
	public Texture2D abilitiesButtonPressed;
	public Texture2D abilitiesBox;
	
	public Texture2D equipmentButton;
	public Texture2D equipmentButtonPressed;
	public Texture2D equipmentBox;
	
	public Texture2D exitButton;
	private Texture2D changingTexture;


	//Creating a matrix scale for different resolutions
	private float originalWidth = 800.0f; 
	private float originalHeight = 400.0f; 
	private Vector3 scale;


	// Use this for initialization
	void OnGUI () {

		scale.x = Screen.width/originalWidth; // calculate horizontal scale
		scale.y = Screen.height/originalHeight; // calculate vert scale
		scale.z = 1;
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scale);

		//Drawing borderLeft from top left
		GUI.skin = TutorialSkin;
		GUI.DrawTexture (new Rect (0, 0, 161, 328), borderLeft);
		GUI.DrawTexture (new Rect (800 - 52, 0, 52, 313), borderRight);
		GUI.DrawTexture (new Rect(16, 36, 728, 363), changingTexture);

		bool overworldnav = false;
		bool combat = false;
		bool abilities = false;
		bool equipment = false;


		//Overworld button
		if (GUI.Button (new Rect (128, 0, 182, 39), overworldnavButton)) {
			changingTexture = overworldnavBox;
		}
		if (GUI.Button (new Rect (278, 0, 182, 39), combatButton)) {
			changingTexture = combatBox;
		}
		if (GUI.Button (new Rect (428, 0, 182, 39), abilitiesButton)) {
			changingTexture = abilitiesBox;
		}
		if (GUI.Button (new Rect (578, 0, 182, 39), equipmentButton)) {
			changingTexture = equipmentBox;
		}


		//Exit Button
		if (GUI.Button (new Rect (800 - 190, 400 - 55, 190, 55), exitButton)) {
			Application.LoadLevel("Overworld");
		}

		GUI.matrix = svMat;
	}

}
