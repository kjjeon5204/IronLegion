using UnityEngine;
using System.Collections;

public class UI2DDragButton : BaseUIButton {
    public GameObject objectToScroll;
    public Camera cameraDrawn;


    //Limit determines the limitation on marker movement
    //ex1. Left marker's x value cannot go lower than left limit's x val
    //ex2. Upper marker cannot go lower than the upper limit
    public Transform leftLimit;
    public Transform rightLimit;
    public Transform upperLimit;
    public Transform lowerLimit;


    //Marker is used to track the edge border of the window
    public Transform leftMarker;
    public Transform rightMarker;
    public Transform upperMarker;
    public Transform lowerMarker;

    public bool panXAxis;
    public bool panYAxis;
    public bool inverseDirection = false;

    public float panSpeed;

    Vector3 panDirection;

    public override void button_held_action(CustomInput myTouch)
    {
        panDirection = Vector3.zero;
        if (myTouch.deltaPosition != Vector3.zero)
        {
            if (panXAxis)
            {
                if (!inverseDirection && rightMarker.position.x > rightLimit.position.x &&
                    myTouch.deltaPosition.x < 0)
                {
                    panDirection += Vector3.left;
                }
                else if (inverseDirection && rightMarker.position.x > rightLimit.position.x &&
                    myTouch.deltaPosition.x > 0)
                {
                    panDirection += Vector3.left;
                }

                if (!inverseDirection && leftMarker.position.x < leftLimit.position.x &&
                    myTouch.deltaPosition.x > 0)
                {
                    panDirection += Vector3.right;
                }
                else if (inverseDirection && leftMarker.position.x < leftLimit.position.x &&
                    myTouch.deltaPosition.x < 0)
                {
                    panDirection += Vector3.right;
                }
            }

            if (panYAxis)
            {
                if (!inverseDirection && lowerMarker.position.y < lowerLimit.position.y &&
                    myTouch.deltaPosition.y > 0)
                {
                    panDirection += Vector3.up;
                }
                else if (lowerMarker.position.y < lowerLimit.position.y &&
                    myTouch.deltaPosition.y < 0)
                {
                    panDirection += Vector3.up;
                }

                if (!inverseDirection && upperMarker.position.y > upperLimit.position.y &&
                    myTouch.deltaPosition.y < 0)
                {
                    panDirection += Vector3.down;
                }
                else if (upperLimit.position.y > upperLimit.position.y &&
                    myTouch.deltaPosition.y < 0)
                {
                    panDirection += Vector3.down;
                }
            }
        }
        panDirection.Normalize();
        objectToScroll.transform.Translate(panDirection * Time.deltaTime * panSpeed);
    }

	// Use this for initialization
	void Start () {
        //Initialize limit conditions and marker conditions through script
	}
}
