using UnityEngine;
using System.Collections;
using System;

public class Slot : MonoBehaviour {
	public string ability_name;
	public int id_num;
	
	private SpriteRenderer filler;
	private SpriteRenderer icon;
	
	public Sprite filler_sprite;
	public Sprite icon_sprite;
	
	private AbilityControls controls;
	private int slot_index;
	public bool close;
	// Use this for initialization
	
	void Awake() {
		ability_name = "-";
		id_num = -1;
	}
	
	void Start () {
		filler = transform.Find("Filler").GetComponent<SpriteRenderer>();
		icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
		controls = GameObject.Find("AbilityFrame").GetComponent<AbilityControls>();
		slot_index = Convert.ToInt32(gameObject.name.Substring(5));
	}
	
	// Update is called once per frame
	void Update () {
		UpdateImage();
	}
	
	void Clicked() {
		controls.SlotClicked(slot_index,close);
	}
	
	public void UpdateImage() {
		if (ability_name == "-")
		{
			filler.color = Color.clear;
			icon.color = Color.clear;
		}
		else
		{
			filler.color = Color.white;
			icon.color = Color.white;
		}
		filler.sprite = filler_sprite;
		icon.sprite = icon_sprite;
	}
}
