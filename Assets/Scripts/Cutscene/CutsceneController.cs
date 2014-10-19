using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

	[System.Serializable]
	public struct CutsceneCamera {
		public Camera cam;
		public float time;
		public Animator anim;
		public bool fade;
	}

	public CutsceneCamera[] scenes;
	int activateHASH = Animator.StringToHash("Activate");
	
	public float time;
	public int counter;
	private float total_time;
	public ScreenFadeIN screen_fade;
	private bool done;
	private bool fading;
	
	void Start () {
		time = 0f;
		counter = 0;
		total_time = scenes[0].time;
		if (scenes[0].anim != null)
		scenes[0].anim.SetBool(activateHASH,true);
		done = false;
		fading = false;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		
		if (!fading && counter < scenes.Length && scenes[counter].fade && time > total_time-1f)
		{
			screen_fade.FadeOut();
			fading = true;
		}
		
		if (time > total_time)
		{
			fading = false;
			Debug.Log("Next camera");
			time = 0f;
			counter++;
			if (counter < scenes.Length)
			{
				if (scenes[counter].anim != null)
					scenes[counter].anim.SetBool(activateHASH,true);
				scenes[counter-1].cam.enabled = false;
				total_time = scenes[counter].time;
				
				if (scenes[counter-1].fade)
					screen_fade.FadeIn();
			}
			else if (!done)
			{
				done = true;
				screen_fade.EndScene();
			}
		}
        if (counter >= scenes.Length || Input.touchCount > 0)
        {
            Application.LoadLevel("Overworld");
        }
	}
}
