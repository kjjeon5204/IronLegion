using UnityEngine;
using System.Collections;

public class BlutSaugerEffect : MyEffect {
    public override void OnEnable()
    {
        effect[0].renderer.enabled = true;
        animation.Play("loop");
    }

    public override void OnDisable()
    {
        effect[0].renderer.enabled = false;
    }

	// Use this for initialization
	public override void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
        if (!animation.IsPlaying("loop"))
        {
            gameObject.SetActive(false);
        }
	}
}
