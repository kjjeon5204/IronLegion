using UnityEngine;
using System.Collections;

public class LibraryAbility : MonoBehaviour {
	public int id;
	private AbilityControls controls;
	private SpriteRenderer icon;
	public Sprite icon_sprite;
	private GameObject library;
	private GameObject highlight;
	// Use this for initialization
	
	void Start() {
		controls = GameObject.Find("AbilityFrame").GetComponent<AbilityControls>();
		icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
		highlight = GameObject.Find("HighlightAbility");
		
	}
	
	// Update is called once per frame
	void Clicked() {
			controls.SetAbilityToSwitch(id);
			highlight.transform.position = gameObject.transform.position;
			highlight.transform.parent = gameObject.transform;
	}
	
	void SetSprite() {
		icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
		icon.sprite = icon_sprite;
		library = GameObject.Find("LibraryControl");
		transform.parent = library.transform;
	}
}
