using UnityEngine;
using System.Collections;

public class UILayering : MonoBehaviour {
    public string sortingLayerName;
    public int sortingOrder;
    Renderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null)
        {
            myRenderer.renderer.sortingLayerName = sortingLayerName;
            myRenderer.renderer.sortingOrder = sortingOrder;
        }
	}
}
