using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

	[System.Serializable]
	public struct CutsceneCamera {
		public Camera cam;
		public float time;
		public Animator anim;
	}

	public CutsceneCamera[] scenes;
	int activateHASH = Animator.StringToHash("Activate");
	
	public float time;
	public int counter;
	private float total_time;
	public ScreenFadeIN screen_fade;
	private bool done;
	
	void Start () {
		time = 0f;
		counter = 0;
		total_time = scenes[0].time;
		if (scenes[0].anim != null)
		scenes[0].anim.SetBool(activateHASH,true);
		done = false;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		
		if (time > total_time)
		{
			Debug.Log("Next camera");
			time = 0f;
			counter++;
			if (counter < scenes.Length)
			{
				if (scenes[counter].anim != null)
					scenes[counter].anim.SetBool(activateHASH,true);
				scenes[counter-1].cam.enabled = false;
				total_time = scenes[counter].time;
			}
			else if (!done)
			{
				done = true;
				screen_fade.EndScene();
			}
		}
	}
}
