using UnityEngine;
using System.Collections;

public class OscillateColor : MonoBehaviour {
	private SpriteRenderer sr;
	private float change = -1f;
	private Vector4 v_change;
	private bool up = false;
	// Use this for initialization
	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		v_change = new Vector4(1f,1f,1f,1f);
	}
	
	
	// Update is called once per frame
	void Update () {
		if (up && v_change.x > 0.95f)
		{
			change = change*-1f;
			up = false;
		}
		else if (!up && v_change.x < 0.3f)
		{
			change = change*-1f;
			up = true;
		}
		v_change.x += change*Time.deltaTime;
		v_change.z += change*Time.deltaTime;
		
		sr.color = (Color)v_change;
	}
}