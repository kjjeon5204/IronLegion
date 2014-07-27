using UnityEngine;
using System.Collections;

public class SetOverworldUI : MonoBehaviour {

	public GameObject hangar;
	public GameObject quit;
	public GameObject settings;
	public GameObject store;
	public GameObject frame;
	public GameObject left_arrow;
	public GameObject right_arrow;
	Camera camera;
	public float zValue = 7f;
	// Use this for initialization
	void Start () {
		zValue = 7f;
		
		camera = GameObject.Find("Camera").GetComponent<Camera>();
		
		Vector3 positionStore = camera.ViewportToWorldPoint(new Vector3(0,1f,zValue));
		Vector3 positionHangar = camera.ViewportToWorldPoint(new Vector3(0f,0f,zValue));
		Vector3 positionSettings = camera.ViewportToWorldPoint(new Vector3(1f,1f,zValue));
		Vector3 positionQuit = camera.ViewportToWorldPoint(new Vector3(1f,0,zValue));
		Vector3 positionFrame = camera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,zValue));
		Vector3 positionLeftArrow = camera.ViewportToWorldPoint(new Vector3(0.01f,0.5f,zValue));
		Vector3 positionRightArrow = camera.ViewportToWorldPoint(new Vector3(0.99f,0.5f,zValue));
		
		store.transform.position = positionStore;
		hangar.transform.position = positionHangar;
		settings.transform.position = positionSettings;
		quit.transform.position = positionQuit;
		frame.transform.position = positionFrame;
		left_arrow.transform.position = positionLeftArrow;
		right_arrow.transform.position = positionRightArrow;
		
		gameObject.GetComponent<SetCamera>().enabled = true;
		
	}
}
