using UnityEngine;
using System.Collections;

public class AllyRightWindow : MonoBehaviour {
	public TextMesh title;
	public TextMesh paragraph;
	
	// Use this for initialization
	void Start () {
		title.GetComponent<Renderer>().sortingLayerName = "Tiles";
		title.text = "Welcome to the\nAllies Menu";
		
		paragraph.GetComponent<Renderer>().sortingLayerName = "Tiles";
		paragraph.text = "Allies that you have\ncurrently unlocked are\nshown here. You can\nchoose which allies to\nbring to the battle.\n\nFuture updates will\ncome with more allies\nto add to your roster.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
