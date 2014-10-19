using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	private SpriteRenderer button;
	public Sprite buttonON;
	public Sprite buttonOFF;
	
	public bool beginClick;
	public bool ignoreSpriteChanging;
    public AudioSource beginClickSound;
    public AudioSource endClickSound;

	// Use this for initialization
	void Start () {
		button = GetComponent<SpriteRenderer>();
		beginClick = false;
		if (!ignoreSpriteChanging)
		button.sprite = buttonOFF;
	}
	
	void BeginClick() {
		beginClick = true;
		if (!ignoreSpriteChanging)
		button.sprite = buttonON;

        if (beginClickSound != null)
            beginClickSound.Play();
	}
	
	void CanceledClick() {
		beginClick = false;
		if (!ignoreSpriteChanging)
		button.sprite = buttonOFF;
	}
	
	void EndClick() {
		if (beginClick)
		{
			beginClick = false;
			gameObject.SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
			if (!ignoreSpriteChanging)
			button.sprite = buttonOFF;

            if (endClickSound != null)
                endClickSound.Play();
		}
	}
}