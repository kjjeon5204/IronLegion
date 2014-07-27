using UnityEngine;
using System.Collections;

public class DialogueControls : MonoBehaviour {
	private DialogueData current_dialogue;
	public SpriteRenderer icon_sprite_renderer;
	public TextMesh dialogue_text;
	public TextMesh name;
	
	private SpriteRenderer current_spriteRenderer;
	
	void Awake() {
		current_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	/*
	void Start() {
		DialogueData empty = new DialogueData();
		empty.dialogueDescription = "";
		SetDialogue(empty);
	}
	*/

	void UpdateDialogue () {
		icon_sprite_renderer.sprite = current_dialogue.characterSprite;
		dialogue_text.text = current_dialogue.text;
		name.text = current_dialogue.characterName;
	}
	
	public void SetDialogue(DialogueData data) {
		current_dialogue = data;
		
		if (current_dialogue.dialogueDescription != "")
		{
			//Debug.Log("Enabled");
			current_spriteRenderer.enabled = true;
			icon_sprite_renderer.enabled = true;
			dialogue_text.renderer.enabled = true;
			name.renderer.enabled = true;
			UpdateDialogue();
		}
		else
		{
			//Debug.Log("Disabled");
			current_spriteRenderer.enabled = false;
			icon_sprite_renderer.enabled = false;
			dialogue_text.renderer.enabled = false;
			name.renderer.enabled = false;
		}
	}
}
