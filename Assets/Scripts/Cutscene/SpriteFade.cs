using UnityEngine;
using System.Collections;

public class SpriteFade : MonoBehaviour {
	private bool fade_in;
	private bool fade_out;
	public float fade_time = 1f;
	private float time;
	
	public SpriteRenderer rend;
	
	// Update is called once per frame
	void Update () {
		if (fade_in)
		FadeIn();
		if (fade_out)
		FadeOut();
	}
	
	void FadeIn() {
		time += Time.deltaTime;
		rend.color = Color.Lerp(Color.clear, Color.white, time/fade_time);
		
		if(rend.color.a >= 0.95f)
		{
			rend.color = Color.white;
			fade_in = false;
		}
	}
	
	void FadeOut() {
		time += Time.deltaTime;
		rend.color = Color.Lerp(Color.white, Color.clear, time/fade_time);
		
		if(rend.color.a <= 0.05f)
		{
			rend.color = Color.clear;
			fade_out = false;
		}
	}
	
	public void BeginFadeIn() {
		fade_in = true;
		time = 0f;
	}
	
	public void BeginFadeOut() {
		fade_out = true;
		time = 0f;
	}
}
