using UnityEngine;
using System.Collections;

public class Model3DSortingSetter : MonoBehaviour {
    public int mySortingOrder;
    public Renderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer.sortingOrder = mySortingOrder;
	}
	
}
