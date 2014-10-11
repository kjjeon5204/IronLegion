using UnityEngine;
using System.Collections;

public class LockedCard : MonoBehaviour {

	public TextMesh cardName;
	public TextMesh description;

	// Use this for initialization
	void Start () {
		cardName.GetComponent<Renderer>().sortingLayerName = "Tiles";
		cardName.text = "Locked";
		
		description.GetComponent<Renderer>().sortingLayerName = "Tiles";
		description.text = "Not yet released";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
