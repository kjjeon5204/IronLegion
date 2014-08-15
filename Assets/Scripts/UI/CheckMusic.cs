using UnityEngine;
using System.Collections;

public class CheckMusic : MonoBehaviour {
	public Settings settings;

	// Use this for initialization
	void Start () {
		settings = new Settings();
	}
	
	// Update is called once per frame
	void Update () {
		settings.load_data();
		if (settings.CheckMusic())
		{
			audio.volume = 1f;
		}
		else
		{
			audio.volume = 0f;
		}
	}
}
