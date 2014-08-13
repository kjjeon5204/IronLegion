using UnityEngine;
using System.Collections;

public class QualityButtonSettings : MonoBehaviour {
	public int quality;
	public TextMesh text_object;
	private Settings settings;
	private SettingsList list_of_settings;
	
	void Start() {
		settings = new Settings();
		list_of_settings = settings.GetSettings();
	}
	
	void Clicked() {
		list_of_settings.quality++;
		if (list_of_settings.quality > 2)
		list_of_settings.quality = 0;
		switch (list_of_settings.quality)
		{
			case 0:
				text_object.text = "Low";
				break;
			case 1:
				text_object.text = "Med";
				break;
			case 2:
				text_object.text = "High";
				break;
			default:
				break;
		}
		settings.save_data();
	}
}
