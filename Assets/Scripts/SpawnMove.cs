using UnityEngine;
using System.Collections;

public class SpawnMove : MonoBehaviour {
	
	private Vector3 StartPos;
	private Vector3 EndPos;
	public float Speed;
	public float duration;
	public GameObject Destination;
	private Vector3 dirction;
	private float localTime;
	public bool ifLoop;
	public AnimationClip obAnimation;
	private string previousAnimation;
	private int phase;
	public GameObject detonator;
	public GameObject DetonatorTriggerMuzzle;
	// Use this for initialization
	void Start () {
		StartPos = transform.position;
		EndPos = Destination.transform.position;
		EndPos.y = StartPos.y;
		dirction = (EndPos - StartPos).normalized;
		transform.LookAt (EndPos);
		localTime = Time.time;
		phase = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (localTime + duration < Time.time) {
			Destroy(gameObject);
		}
		if ((transform.position - EndPos).magnitude > 3){
			transform.Translate (Vector3.forward * Speed * Time.deltaTime); 
		}
		else {
			if (animation != null){
				if (phase == 0){
					animation.Play(obAnimation.name);
					previousAnimation = obAnimation.name;
					phase ++;
					if (detonator)
						Instantiate(detonator, DetonatorTriggerMuzzle.transform.position, Quaternion.identity);
				}
				if (!animation.IsPlaying(previousAnimation) && phase == 1)
				{
					if (!ifLoop){	
						Destroy(gameObject);
					}
					transform.LookAt (EndPos);
					transform.position = StartPos;
					phase = 0;
				}
			}
			else {
				if (!ifLoop){	
					Destroy(gameObject);
				}
				transform.LookAt (EndPos);
				transform.position = StartPos;
			}
		}
	}
	
}
