using UnityEngine;
using System.Collections;

public class TutorialControls : MonoBehaviour {
	
	public bool[] tutorials_on;
	
	public DialogueData[] notice_dialogue;
	public SpriteRenderer icon_sprite_renderer;
	public TextMesh name;
	public TextMesh dialogue_text;
	private PlayerDataReader tutorial_data;
	
	public GameObject notice;
	private Vector3 notice_position;
	public GameObject waiting;
	
	void Awake () {
		tutorials_on = new bool[3];
		tutorials_on[0] = false;
		tutorials_on[1] = false;
		tutorials_on[2] = false;
		
		tutorial_data = new PlayerDataReader();
		
		notice_position = new Vector3(60f,-2.5f,5f);
	}
	
	public void ActivateTutorials() {
		if (tutorials_on[0] && !tutorial_data.check_event_played("hangar_first"))
		{
			icon_sprite_renderer.sprite = notice_dialogue[0].characterSprite;
			dialogue_text.text = notice_dialogue[0].text;
			name.text = notice_dialogue[0].characterName;
			NoticeOnScreen();
		}
		if (tutorials_on[1] && !tutorial_data.check_event_played("hangar_second"))
		{
			icon_sprite_renderer.sprite = notice_dialogue[1].characterSprite;
			dialogue_text.text = notice_dialogue[1].text;
			name.text = notice_dialogue[1].characterName;
			NoticeOnScreen();
		}
		if (tutorials_on[2] && !tutorial_data.check_event_played("store"))
		{
			icon_sprite_renderer.sprite = notice_dialogue[2].characterSprite;
			dialogue_text.text = notice_dialogue[2].text;
			name.text = notice_dialogue[2].characterName;
			NoticeOnScreen();
		}
	}
	
	void NoticeOnScreen() {
		notice.transform.position = notice_position;
	}
}
