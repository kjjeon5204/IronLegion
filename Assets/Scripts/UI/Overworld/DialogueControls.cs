using UnityEngine;
using System.Collections;

public class DialogueControls : MonoBehaviour {
	public DialogueData[] current_dialogue;
	public SpriteRenderer icon_sprite_renderer;
	public TextMesh dialogue_text;
	public TextMesh name;
	public TextMesh continue_text;
	public int text_number;
	public bool more_text;
	private bool is_text;
	
	private SpriteRenderer current_spriteRenderer;
	
	void Awake() {
		current_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		text_number = 0;
	}
	
	/*
	void Start() {
		DialogueData empty = new DialogueData();
		empty.dialogueDescription = "";
		SetDialogue(empty);
	}
	*/
	
	void Clicked() {
		text_number++;
		if (text_number >= current_dialogue.Length-1)
		{
			text_number = current_dialogue.Length-1;
			more_text = false;
		}
		if (is_text)
		UpdateDialogue();
	}

	void UpdateDialogue () {
		if (more_text)
		{
			continue_text.renderer.enabled = true;
		}
		else
		{
			continue_text.renderer.enabled = false;
		}
		icon_sprite_renderer.sprite = current_dialogue[text_number].characterSprite;
		dialogue_text.text = current_dialogue[text_number].text;
		name.text = current_dialogue[text_number].characterName;
	}
	
	public void SetDialogue(DialogueData[] data) {
		current_dialogue = data;
		text_number = 0;
		if (data.Length > 1)
		{
			more_text = true;
			is_text = true;
		}
		else if (data.Length == 1)
		{
			more_text = false;
			is_text = true;
		}
		else
		{
			more_text = false;
			is_text = false;
		}
		
		if (is_text) //&& current_dialogue[text_number].dialogueDescription != "")
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
			continue_text.renderer.enabled = false;
		}
	}
}
