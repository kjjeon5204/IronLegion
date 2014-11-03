using UnityEngine;
using System.Collections;

public class UI2DController : MonoBehaviour {
    public LayerMask layerClicked;
    public Camera clickCam;

    GameObject itemCurTouching;

	// Use this for initialization
	void Start () {
        if (clickCam == null)
        {
            if (Camera.main != null)
                clickCam = Camera.main;
        }
	}

    void activate_button_touch(Touch curTouch)
    {
        Vector3 touchPos = clickCam.ScreenToWorldPoint(curTouch.position);
        RaycastHit2D myRayCast;
        if (layerClicked == LayerMask.NameToLayer("Nothing") ||
            layerClicked == LayerMask.NameToLayer("Everything"))
        {
            myRayCast = Physics2D.Raycast(touchPos, Vector3.forward, 50.0f);
        }
        else
        {
            myRayCast = Physics2D.Raycast(touchPos, Vector3.forward, 50.0f, layerClicked);
        }
    }

    void deactivate_button_touch(Touch curTouch)
    {

    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
