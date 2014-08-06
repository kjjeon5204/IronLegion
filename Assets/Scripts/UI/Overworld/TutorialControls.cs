using UnityEngine;
using System.Collections;

public class TutorialControls : MonoBehaviour {
	
	//FOR TESTING PURPOSES
	public bool DISABLE_TUTORIALS;
	
	public bool[] tutorials_on;
	
	public DialogueData[] notice_dialogue;
	public SpriteRenderer icon_sprite_renderer;
	public TextMesh name;
	public TextMesh dialogue_text;
	private PlayerDataReader tutorial_data;
	
	public GameObject notice;
	private Vector3 notice_position;
	public GameObject waiting;
	public GameObject hangar_activate;
	public GameObject store_activate;
	
	public OscillateColor hangar_color;
	public OscillateColor store_color;
	
	public Button dialogue_button;
	
	void Awake () {
		tutorials_on = new bool[5];
		tutorials_on[0] = false;
		tutorials_on[1] = false;
		tutorials_on[2] = false;
		tutorials_on[3] = false;
		tutorials_on[4] = false;
		
		tutorial_data = new PlayerDataReader();
		
		notice_position = new Vector3(60f,-2.5f,5f);
	}
	
	public void ActivateTutorials() {
		notice.transform.position = notice_position;
		if (!DISABLE_TUTORIALS)
		{
			if (tutorials_on[0] && !tutorial_data.check_event_played("hangar_first"))
			{
				icon_sprite_renderer.sprite = notice_dialogue[0].characterSprite;
				dialogue_text.text = notice_dialogue[0].text;
				name.text = notice_dialogue[0].characterName;
				TutorialAction("HANGAR");
			}
			if (tutorials_on[1] && !tutorial_data.check_event_played("hangar_second"))
			{
				icon_sprite_renderer.sprite = notice_dialogue[1].characterSprite;
				dialogue_text.text = notice_dialogue[1].text;
				name.text = notice_dialogue[1].characterName;
				TutorialAction("HANGAR");
			}
			if (tutorials_on[2] && !tutorial_data.check_event_played("store"))
			{
				icon_sprite_renderer.sprite = notice_dialogue[2].characterSprite;
				dialogue_text.text = notice_dialogue[2].text;
				name.text = notice_dialogue[2].characterName;
				TutorialAction("STORE");
			}
			if (tutorials_on[3] && !tutorial_data.check_event_played("first_tile"))
			{
				icon_sprite_renderer.sprite = notice_dialogue[3].characterSprite;
				dialogue_text.text = notice_dialogue[3].text;
				name.text = notice_dialogue[3].characterName;
				TutorialAction("TILE");
			}
			if (tutorials_on[4] && !tutorial_data.check_event_played("second_tile"))
			{
				icon_sprite_renderer.sprite = notice_dialogue[4].characterSprite;
				dialogue_text.text = notice_dialogue[4].text;
				name.text = notice_dialogue[4].characterName;
				TutorialAction("TILE2");
			}
		}
	}
	
	void TutorialAction(string type) {
		notice.transform.position = notice_position;
		if (type == "HANGAR")
		{
			hangar_activate.transform.position = notice_position;
			hangar_color.enabled = true;
		}
		else if (type == "STORE")
		{
			store_activate.transform.position = notice_position;
			store_color.enabled = true;
		}
		else if (type == "TILE")
		{
			//Do nothing at the moment
		}
		else if (type == "TILE2")
		{
			dialogue_button.enabled = true;
		}
	}
}
