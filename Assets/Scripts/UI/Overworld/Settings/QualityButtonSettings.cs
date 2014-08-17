using UnityEngine;
using System.Collections;

public class QualityButtonSettings : MonoBehaviour {
	public int quality;
	public TextMesh text_object;
	private Settings settings;
	
	void Start() {
		settings = new Settings();
		quality = settings.GetQuality();
	}
	
	void Clicked() {
		quality++;
		if (quality > 2)
		quality = 0;
		switch (quality)
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
		settings.SetQuality(quality);
		settings.save_data();
	}
}
