using UnityEngine;
using System.Collections;

public class AnimationLooper : MonoBehaviour {
	public AnimationClip animationLoop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		animation.Play (animationLoop.name);
	}
}
