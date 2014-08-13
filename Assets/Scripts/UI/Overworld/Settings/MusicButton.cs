using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour {
	
	private SpriteRenderer icon;
	public Sprite iconON;
	public Sprite iconOFF;
	private Settings settings;
	private SettingsList list_of_settings;

	void Start () {
		icon = GameObject.Find("Music Icon").GetComponent<SpriteRenderer>();
		
		settings = new Settings();
		list_of_settings = settings.GetSettings();
		list_of_settings.music = true;
		if (list_of_settings.music)
		icon.sprite = iconON;
		else
		icon.sprite = iconOFF;
	}
	
	void Clicked() {
		list_of_settings.music = !list_of_settings.music;
		if (list_of_settings.music)
		icon.sprite = iconON;
		else
		icon.sprite = iconOFF;
		
		settings.save_data();
	}
}
