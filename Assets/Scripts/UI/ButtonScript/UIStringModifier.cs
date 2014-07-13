using UnityEngine;
using System.Collections;

public class UIStringModifier : MonoBehaviour {
    string textToDisplay;
    TextMesh textMesh;

    public void initialize_text(string input)
    {
        textMesh = GetComponent<TextMesh>();
        textToDisplay = input;
        textMesh.text = textToDisplay;
    }

    public bool is_blank()
    {
        if (textMesh.text.Length == 0)
        {
            return true;
        }
        return false;
    }

	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = textToDisplay;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
