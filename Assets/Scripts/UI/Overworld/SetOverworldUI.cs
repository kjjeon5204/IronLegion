using UnityEngine;
using System.Collections;

public class SetOverworldUI : MonoBehaviour {

	GameObject hangar;
	GameObject quit;
	GameObject settings;
	GameObject store;
	GameObject frame;
	Camera camera;
	public float zValue = 7f;
	// Use this for initialization
	void Start () {
		hangar = GameObject.Find("Hangar");
		quit = GameObject.Find("Quit");
		settings = GameObject.Find("Settings");
		store = GameObject.Find("Store");
		frame = GameObject.Find("Overworld Frame");
		zValue = 7f;
		
		camera = GameObject.Find("Camera").GetComponent<Camera>();
		
		Vector3 positionStore = camera.ViewportToWorldPoint(new Vector3(0,1f,zValue));
		Vector3 positionHangar = camera.ViewportToWorldPoint(new Vector3(0f,0f,zValue));
		Vector3 positionSettings = camera.ViewportToWorldPoint(new Vector3(1f,1f,zValue));
		Vector3 positionQuit = camera.ViewportToWorldPoint(new Vector3(1f,0,zValue));
		Vector3 positionFrame = camera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,zValue));
		
		store.transform.position = positionStore;
		hangar.transform.position = positionHangar;
		settings.transform.position = positionSettings;
		quit.transform.position = positionQuit;
		frame.transform.position = positionFrame;
		
		gameObject.GetComponent<SetCamera>().enabled = true;
		
	}
}
