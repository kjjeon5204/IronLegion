using UnityEngine;
using System.Collections;

public class MyTextMesh : MonoBehaviour {
    public string sortingLayer;
    public int orderInLayer;
    TextMesh myMesh;

	// Use this for initialization
	void Start () {
        myMesh = GetComponent<TextMesh>();
        myMesh.renderer.sortingLayerName = sortingLayer;
        myMesh.renderer.sortingOrder = orderInLayer;
	}
	
}
