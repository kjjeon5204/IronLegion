using UnityEngine;
using System.Collections;

public class SpriteModifier : MonoBehaviour {
    public Sprite[] spriteStates;
    public SpriteRenderer spriteRenderer;

    public void switch_sprite(int spriteAccessNum)
    {
        spriteRenderer.sprite = spriteStates[spriteAccessNum];
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
