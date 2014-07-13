/*using UnityEngine;
using System.Collections;

public class OverworldGUI : MonoBehaviour {
	//this script controls the gui of the overworld
	//the basic idea is to use an enum to control
	//which gui assets are displayed so you can switch 
	//between different screens (except combat) w/o
	//actually changin scene to avoid hassle

	MapData mapData;
	HeroStats heroStats = new HeroStats();

	//art assets
	public Texture startButton;
	public Texture startBackground;
	public Texture tutorialButton;
	public Texture tutorialButtonP;
	public Texture equipButton;
	public Texture equipButtonP;
	public Texture exitButton;
	public Texture exitButtonP;
	public Texture instructions;
	public Texture solHexButton;
	public Texture solHexButtonP;
	public Texture popupBackground;
	public Texture yesButton;
	public Texture yesButtonP;
	public Texture noButton;
	public Texture noButtonP;
	public Texture mainBackground;
	public Texture levelButton;
	public Font font;

	//font color
	private const int fred = 0xFF;
	private const int fgreen = 0xA8;
	private const int fblue = 0x00;
	private Color yellowOrange = new Color((float)(fred) / 255, (float)(fgreen) / 255, 
	                                       (float)(fblue) / 255, 1);

	//drawing variables
	private float tutorialWidthPercent = 0.3f; //set this one
	private float equipWidthPercent = 1.0f;
	private float levelWidthPercent = 1.0f;
	private float exitWidthPercent = 1.0f;
	private float instructionsWidthPercent = 1.0f;
	private float hexWidthPercent = 0.125f; //set this one
	private float hexHeightPercent = 1.0f;
	private float popupWidthPercent = 1.0f;
	private float yesWidthPercent = 1.0f;
	private float noWidthPercent = 1.0f;
	private bool equipPressed = false;
	private bool tutorialPressed = false;
	private bool exitPressed = false;
	private bool yesPressed = false;
	private bool noPressed = false;
	private Rect startRect;
	private Rect tutorialRect;
	private Rect equipRect;
	private Rect exitRect;
	private Rect instructionsRect;
	private Rect popupRect;
	private Rect yesRect;
	private Rect noRect;
	private Rect levelRect;
	private Rect levelRect2;
	private GUIStyle levelStyle;
	private GUIStyle hexStyle;
	private GUIStyle assaultStyle;
	private HexButton[] hex;
	private int numHexes = 7;

	//classes/structs/enums
	enum ScreenMode {
		//to simulate switching screens, we use a switch statement on this
		//enum to control what GUIs are display
		START, OVERWORLD, TUTORIAL, EQUIP, SLIDING
	}
	private class HexButton {
	//combines level with a button texture
	//uses level to lookup important info
		public HexButton() {
			level = -1;
			button = new Texture();
			buttonP = new Texture();
			rect = new Rect();
			pressRect = new Rect();
			pressed = false;
		}

		public HexButton(int l, Texture b, Texture bp) {
			level = l;
			button = b;
			buttonP = bp;
			rect = new Rect();
			pressRect = new Rect();
			pressed = false;
		}

		public void setHex(int l, Texture b, Texture bp) {
			level = l;
			button = b;
			buttonP = bp;
		}

		public int level;
		public Texture button;
		public Texture buttonP;
		public Rect rect;
		public Rect pressRect;
		public bool pressed;
	}


	//control variables
	private bool hexSelected = false;
	private int levelSelected = -1;
	private bool initialized = false;
	private ScreenMode screenMode = ScreenMode.START;
	private int slideTimeStart = -1;
	private bool yesNoReady = false;

	private void initialize() {

		//screen percents
		equipWidthPercent = tutorialWidthPercent * equipButton.width / tutorialButton.width;
		levelWidthPercent = tutorialWidthPercent * levelButton.width / tutorialButton.width;
		exitWidthPercent = tutorialWidthPercent * exitButton.width / tutorialButton.width;
		instructionsWidthPercent = tutorialWidthPercent * instructions.width / tutorialButton.width;
		hexHeightPercent = hexWidthPercent * solHexButton.height / solHexButton.width;
		popupWidthPercent = 4.5f * hexWidthPercent;
		yesWidthPercent = popupWidthPercent * yesButton.width / popupBackground.width;
		noWidthPercent = popupWidthPercent * noButton.width / popupBackground.width;

		//corner buttons
		tutorialRect = new Rect(0, 0, tutorialWidthPercent * Screen.width, tutorialWidthPercent * Screen.width * tutorialButton.height / (float)(tutorialButton.width));
		equipRect = new Rect(0, Screen.height - equipWidthPercent * Screen.width * equipButton.height / (float)(equipButton.width), 
		                     equipWidthPercent * Screen.width, equipWidthPercent * Screen.width * equipButton.height / (float)(equipButton.width));
		exitRect = new Rect(Screen.width - Screen.width * exitWidthPercent, Screen.height - Screen.width * exitWidthPercent * exitButton.height / exitButton.width, Screen.width * exitWidthPercent, Screen.width * exitWidthPercent * exitButton.height / exitButton.width);
		levelRect = new Rect(Screen.width - Screen.width * levelWidthPercent, 0, Screen.width * levelWidthPercent, Screen.width * levelWidthPercent * levelButton.height / levelButton.width);
		levelRect2 = new Rect(levelRect.x, 0, .9f * levelRect.width, levelRect.height);
		instructionsRect = new Rect(0.5f * (1 - instructionsWidthPercent) * Screen.width, Screen.height - Screen.width * instructionsWidthPercent * instructions.height / instructions.width, 
		                            Screen.width * instructionsWidthPercent, Screen.width * instructionsWidthPercent * instructions.height / instructions.width); 

		//hex buttons
		hex = new HexButton[8];
		for (int i = 1; i <= numHexes; i++) {
			hex[i] = new HexButton();
			hex[i].setHex(i, solHexButton, solHexButtonP);
		}
		float hexw = hexWidthPercent * Screen.width;
		float hexh = hexHeightPercent * Screen.width;
		hex[1].rect = new Rect ((Screen.width - hexw) / 2, (Screen.height - hexh) / 2, hexw, hexh);
		hex[2].rect = new Rect (hex[1].rect.x, hex[1].rect.y - hexh, hexw, hexh);
		hex[3].rect = new Rect (hex[1].rect.x + 0.75f * hexw, hex[1].rect.y - hexh / 2, hexw, hexh);
		hex[4].rect = new Rect (hex[1].rect.x + 0.75f * hexw, hex[1].rect.y + hexh / 2, hexw, hexh);
		hex[5].rect = new Rect (hex[1].rect.x, hex[1].rect.y + hexh, hexw, hexh);
		hex[6].rect = new Rect (hex[1].rect.x - 0.75f * hexw, hex[1].rect.y + hexh / 2, hexw, hexh);
		hex[7].rect = new Rect (hex[1].rect.x - 0.75f * hexw, hex[1].rect.y - hexh / 2, hexw, hexh);
		for (int i = 1; i <= numHexes; i++) {
			hex[i].pressRect = new Rect(hex[i].rect.x + hexw / 4, hex[i].rect.y, hexw / 2, hexh);
		}

		//other rectangles
		startRect = new Rect((Screen.width - startButton.width) / 2, (Screen.height - startButton.height) / 2,
		                     startButton.width, startButton.height);
		popupRect = new Rect((Screen.width - popupBackground.width) / 2, (Screen.height - popupBackground.height) / 2, 
		                     popupBackground.width, popupBackground.height);
		popupRect = new Rect(0.5f * (Screen.width - Screen.width * popupWidthPercent), 0.5f * (Screen.height - Screen.width * popupWidthPercent * popupBackground.height / popupBackground.width),
		                     Screen.width * popupWidthPercent, Screen.width * popupWidthPercent * popupBackground.height / popupBackground.width);
		yesRect = new Rect(Screen.width / 2 - Screen.width * yesWidthPercent / 2 - popupRect.width / 5, 
		                   Screen.height / 2 - Screen.width * yesWidthPercent * yesButton.height / yesButton.width / 2 + popupRect.height / 4,
		                   Screen.width * yesWidthPercent, Screen.width * yesWidthPercent * yesButton.height / yesButton.width);
		noRect = new Rect(Screen.width / 2 - Screen.width * noWidthPercent / 2 + popupRect.width / 5, 
		                  Screen.height / 2 - Screen.width * noWidthPercent * noButton.height / noButton.width / 2 + popupRect.height / 4,
		                  Screen.width * noWidthPercent, Screen.width * noWidthPercent * noButton.height / noButton.width);

		//styles
		levelStyle = new GUIStyle();
		levelStyle.font = font;
		levelStyle.fontSize = Screen.width / 40;
		levelStyle.alignment = TextAnchor.MiddleRight;
		levelStyle.normal.textColor = yellowOrange;
		hexStyle = new GUIStyle();
		hexStyle.font = font;
		hexStyle.fontSize = Screen.width / 100;
		hexStyle.alignment = TextAnchor.MiddleCenter;
		hexStyle.normal.textColor = yellowOrange;
		assaultStyle = new GUIStyle();
		assaultStyle.font = font;
		assaultStyle.fontSize = (int)(Screen.width * popupWidthPercent / 15);
		assaultStyle.alignment = TextAnchor.UpperCenter;

		initialized = true;
	}



	void OnGUI() {

		if (!initialized) {
			initialize();
		}

		switch (screenMode) {
		case ScreenMode.START:

			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), startBackground, ScaleMode.ScaleAndCrop);

			//flicker
			if ((int)(2 * Time.time) % 2 == 0) {
				GUI.DrawTexture(startRect, startButton);
			}
			if (Event.current.type == EventType.MouseUp) {
				screenMode = ScreenMode.SLIDING;
			}

			break;
	
		case ScreenMode.OVERWORLD:

			//draw unpressable objects
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainBackground, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(instructionsRect, instructions);
			GUI.DrawTexture(levelRect, levelButton);
			GUI.Label(levelRect2, "" + heroStats.load_data().level, levelStyle);

			//draw pressable buttons
			if (tutorialPressed) {
				GUI.DrawTexture(tutorialRect, tutorialButtonP);
			} else {
				GUI.DrawTexture(tutorialRect, tutorialButton);
			}
			if (equipPressed) {
				GUI.DrawTexture(equipRect, equipButtonP);
			} else {
				GUI.DrawTexture(equipRect, equipButton);
			}
			if (exitPressed) {
				GUI.DrawTexture(exitRect, exitButtonP);
			} else {
				GUI.DrawTexture(exitRect, exitButton);
			}
			for (int i = 1; i <= numHexes; i++) {
				if (hex[i].pressed) {
					GUI.DrawTexture(hex[i].rect, hex[i].buttonP);
				} else {
					GUI.DrawTexture(hex[i].rect, hex[i].button);
				}
				GUI.Label(hex[i].rect, "Level " + hex[i].level + "\n" + mapData.get_map_data(hex[i].level).clearCount + " Battles", hexStyle);
			}
			if (!hexSelected) {
				tutorialPressed = false;
				equipPressed = false;
				exitPressed = false;
				for (int i = 1; i <= numHexes; i++) {
					hex[i].pressed = false;
				}
				foreach (Touch touch in Input.touches) {
					Vector2 v = touch.position;
					v.y = Screen.height - v.y;
					if (tutorialRect.Contains(v)) {
						tutorialPressed = true;
						if (touch.phase == TouchPhase.Ended) {
							screenMode = ScreenMode.TUTORIAL;
						}
					}
					if (equipRect.Contains(v)) {
						equipPressed = true;
						if (touch.phase == TouchPhase.Ended) {
							screenMode = ScreenMode.EQUIP;
						}
					}
					if (exitRect.Contains(v)) {
						exitPressed = true;
						if (touch.phase == TouchPhase.Ended) {
							Application.Quit();
						}
					}
					for (int i = 1; i <= numHexes; i++) {
						if (hex[i].pressRect.Contains(v)) {
							hex[i].pressed = true;
							if (touch.phase == TouchPhase.Ended) {
								levelSelected = hex[i].level;
								hexSelected = true;
							}
						}
					}
				}

			} else {

				//display assault zone popup
				GUI.DrawTexture(popupRect, popupBackground);
				assaultStyle.normal.textColor = yellowOrange;
				GUI.Label(new Rect((Screen.width - popupRect.width) / 2, (Screen.height - .8f * popupRect.height) / 2,
				                   popupRect.width, popupRect.height), "Assault Zone?", assaultStyle);
				assaultStyle.normal.textColor = Color.white;
				GUI.Label(new Rect((Screen.width - popupRect.width) / 2, (Screen.height - .4f * popupRect.height) / 2,
				                   popupRect.width, popupRect.height), "Level: " + levelSelected, assaultStyle);

				//draw yes/no buttons
				if (yesPressed) {
					GUI.DrawTexture(yesRect, yesButtonP);
				} else {
					GUI.DrawTexture(yesRect, yesButton);
				}
				if (noPressed) {
					GUI.DrawTexture(noRect, noButtonP);
				} else {
					GUI.DrawTexture(noRect, noButton);
				}
				yesPressed = false;
				noPressed = false;
				foreach (Touch touch in Input.touches) {
					if (touch.phase == TouchPhase.Began) {
						yesNoReady = true;
					}
				}
				if (yesNoReady) {
					foreach (Touch touch in Input.touches) {
						Vector2 v = touch.position;
						v.y = Screen.height - v.y;
						if (yesRect.Contains(v)) {
							yesPressed = true;
							if (touch.phase == TouchPhase.Ended) {
								yesPressed = false;
								noPressed = false;
								yesNoReady = false;
								hexSelected = false;
								mapData.store_map_data(levelSelected - 1);
								Application.LoadLevel(2);
							}
						} else if (noRect.Contains(v)) {
							noPressed = true;
							if (touch.phase == TouchPhase.Ended) {
								yesPressed = false;
								noPressed = false;
								yesNoReady = false;
								hexSelected = false;
							}
						}
					}
				}

			}
			break;

		case ScreenMode.TUTORIAL:
			if (GUI.Button (new Rect(Screen.width / 4, Screen.height / 4, 
			                         Screen.width / 2, Screen.height / 2), "Nobody uses tutorials (back)")) {
				screenMode = ScreenMode.OVERWORLD;
			}
			break;

		case ScreenMode.EQUIP:
            Application.LoadLevel(1);
			break;

		case ScreenMode.SLIDING:
			//background
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainBackground, ScaleMode.ScaleAndCrop);

			//determine sliding placement of buttons
			int speed = 200;
			if (slideTimeStart == -1) {
				slideTimeStart = (int)(Time.time * speed);
			}
			int currentTime = (int)(Time.time * speed);
			float percentIn = (currentTime - slideTimeStart) / 100F;
			Rect tutorialRectIn = new Rect((percentIn - 1) * tutorialRect.width, 0, tutorialRect.width, tutorialRect.height);
			Rect equipRectIn = new Rect((percentIn - 1) * equipRect.width, Screen.height - equipRect.height, equipRect.width, equipRect.height);
			Rect exitRectIn = new Rect(Screen.width - percentIn * exitRect.width, Screen.height - exitRect.height, exitRect.width, exitRect.height);
			Rect instructionsRectIn = new Rect((Screen.width - instructionsRect.width) / 2, Screen.height - percentIn * instructionsRect.height, 
			                                   instructionsRect.width, instructionsRect.height);
			Rect levelRectIn = new Rect(Screen.width - percentIn * levelRect.width, 0, levelRect.width, levelRect.height);
			Rect levelRect2In = new Rect(Screen.width - percentIn * levelRect.width, 0, .9f * levelRect.width, levelRect.height);

			//display buttons
			GUI.DrawTexture(instructionsRectIn, instructions);
			GUI.DrawTexture(tutorialRectIn, tutorialButton);
			GUI.DrawTexture(equipRectIn, equipButton);
			GUI.DrawTexture(exitRectIn, exitButton);
			for (int i = 1; i <= numHexes; i++) {
				GUI.DrawTexture(hex[i].rect, hex[i].button);
				GUI.Label(hex[i].rect, "Level " + hex[i].level + "\n" + mapData.get_map_data(hex[i].level).clearCount + " Battles", hexStyle);
			}
			GUI.DrawTexture(levelRectIn, levelButton);
			GUI.Label(levelRect2In, "" + heroStats.load_data().level, levelStyle);

			if (percentIn >= 1F) {
				slideTimeStart = -1;
				screenMode = ScreenMode.OVERWORLD;
			}
			break;
		}
	}
	
}

*/