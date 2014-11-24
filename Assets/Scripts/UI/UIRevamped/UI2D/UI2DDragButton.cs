using UnityEngine;
using System.Collections;

public class UI2DDragButton : BaseUIButton {
    public GameObject objectToScroll;
    public Camera cameraDrawn;

    public Transform leftLimit;
    public Transform rightLimit;
    public Transform upperLimit;
    public Transform lowerLimit;

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

                if (!inverseDirection && upperLimit.position.y > upperLimit.position.y &&
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
        Vector3 lowerRightCorner = new Vector3(1.0f, 1.0f, 0.0f);
        Vector3 upperLeftCorner = Vector3.zero;

        lowerRightCorner = cameraDrawn.ViewportToWorldPoint(lowerRightCorner);
        upperLeftCorner = cameraDrawn.ViewportToWorldPoint(upperLeftCorner);
	}
}
