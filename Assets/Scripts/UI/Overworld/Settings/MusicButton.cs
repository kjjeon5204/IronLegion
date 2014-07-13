using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour {
	
	private SpriteRenderer icon;
	public Sprite iconON;
	public Sprite iconOFF;
	public bool music;
	// Use this for initialization
	void Start () {
		icon = GameObject.Find("Music Icon").GetComponent<SpriteRenderer>();
		music = true;
		if (music)
		icon.sprite = iconON;
		else
		icon.sprite = iconOFF;
	}
	
	void Clicked() {
		music = !music;
		if (music)
		icon.sprite = iconON;
		else
		icon.sprite = iconOFF;
	}
}
