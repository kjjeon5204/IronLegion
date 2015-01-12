using UnityEngine;
using System.Collections;

public class MapEngageWindow : MonoBehaviour {
    public TextMesh[] messageDisplay;
    public SpriteRenderer mySpriteRednerer;

    public void set_engage_window(string[] messageList, Sprite mapPicture)
    {
        int ctr = 0;
        while (ctr < messageDisplay.Length && ctr < messageList.Length)
        {
            messageDisplay[ctr].text = messageList[ctr];
            ctr++;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
