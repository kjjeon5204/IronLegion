using UnityEngine;
using System.Collections;

public class MapZoomSliderControls : BaseUIButton {
    public GameObject mLeftLimit;
    public GameObject mRightLimit;
    public GameObject mTopLimit;
    public GameObject mBottomLimit;

    public Camera mapDisplayCamera;
    public SpriteRenderer cameraDrawWindow;
    public GameObject cLeftMarker;
    public GameObject cRightMarker;
    public GameObject cTopMarker;
    public GameObject cBottomMarker;

    public float cameraZoomSpeed;
    public float cameraMoveSpeed;

    public void set_camera_marker()
    {
       
        float aspectRatio = 1.0f * cameraDrawWindow.bounds.size.x / cameraDrawWindow.bounds.size.y;
        Debug.Log(mapDisplayCamera.orthographicSize);
        Vector3 markerPos = mapDisplayCamera.transform.position;
        markerPos.x -= aspectRatio * mapDisplayCamera.orthographicSize;
        cLeftMarker.transform.position = markerPos;

        markerPos.x += 2.0f * aspectRatio * mapDisplayCamera.orthographicSize;
        cRightMarker.transform.position = markerPos;

        markerPos = mapDisplayCamera.transform.position;
        markerPos.y += mapDisplayCamera.orthographicSize;
        cTopMarker.transform.position = markerPos;

        markerPos.y -= 2 * mapDisplayCamera.orthographicSize;
        cBottomMarker.transform.position = markerPos;
    }
    float mapLength;
    float mapHeight;

    public void initialize_camera()
    {
        set_camera_marker();
        float camLength = cRightMarker.transform.position.x - cLeftMarker.transform.position.x;
        float camHeight = cTopMarker.transform.position.y - cBottomMarker.transform.position.y;
        mapLength = mRightLimit.transform.position.x - mLeftLimit.transform.position.x;
        mapHeight = mTopLimit.transform.position.y - mBottomLimit.transform.position.y;
        
        while (camLength > (1.0f / 2.0f) * mapLength ||
            camHeight > (1.0f / 2.0f) * mapHeight) 
            {
                Debug.Log("Length: " + camLength + " " + mapLength);
                Debug.Log("Height: " + camHeight + " " + mapHeight);
            //recalculate & diminish cam size by 3/4
            mapDisplayCamera.orthographicSize = mapDisplayCamera.orthographicSize * (3.0f / 4.0f);
            set_camera_marker();
            camLength = cRightMarker.transform.position.x - cLeftMarker.transform.position.x;
            camHeight = cTopMarker.transform.position.y - cBottomMarker.transform.position.y;
        }
         
        //Calculate camera limit and reposition camera
        //reposition camera
        if (cLeftMarker.transform.position.x < mLeftLimit.transform.position.x)
        {
            Vector3 movementVector = mLeftLimit.transform.position - cLeftMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cRightMarker.transform.position.x > mRightLimit.transform.position.x)
        {
            Vector3 movementVector = mRightLimit.transform.position - cRightMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cTopMarker.transform.position.y > mTopLimit.transform.position.y)
        {
            Vector3 movementVector = mTopLimit.transform.position - cTopMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cBottomMarker.transform.position.y < mBottomLimit.transform.position.y)
        {
            Vector3 movementVector = mBottomLimit.transform.position - cBottomMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
    }

    public void camera_zoom(float zoomSpeed)
    {
        float camLength = cRightMarker.transform.position.x - cLeftMarker.transform.position.x;
        float camHeight = cTopMarker.transform.position.y - cBottomMarker.transform.position.y;

        if (zoomSpeed > 0)
        {
            if (camLength < mapLength &&
                camHeight < mapHeight)
                mapDisplayCamera.orthographicSize += zoomSpeed * Time.deltaTime;
        }

        else if (zoomSpeed < 0)
        {
            if (mapDisplayCamera.orthographicSize > 2.0f)
                mapDisplayCamera.orthographicSize += zoomSpeed * Time.deltaTime;
        }

        //Recalculate all marker position
        cLeftMarker.transform.position = mapDisplayCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
        cRightMarker.transform.position = mapDisplayCamera.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f));
        cTopMarker.transform.position = mapDisplayCamera.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f));
        cBottomMarker.transform.position = mapDisplayCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));

        //reposition camera
        if (cLeftMarker.transform.position.x < mLeftLimit.transform.position.x)
        {
            Vector3 movementVector = mLeftLimit.transform.position - cLeftMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cRightMarker.transform.position.x > mRightLimit.transform.position.x)
        {
            Vector3 movementVector = mRightLimit.transform.position - cRightMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cTopMarker.transform.position.y > mTopLimit.transform.position.y)
        {
            Vector3 movementVector = mTopLimit.transform.position - cTopMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cBottomMarker.transform.position.y < mBottomLimit.transform.position.y)
        {
            Vector3 movementVector = mBottomLimit.transform.position - cBottomMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
    }

    Vector3 check_valid_map_movement(Vector3 movement)
    {
        if (cLeftMarker.transform.position.x + movement.x < mLeftLimit.transform.position.x)
        {
            movement.x = 0.0f;
        }
        if (cRightMarker.transform.position.x + movement.x > mRightLimit.transform.position.x)
        {
            movement.x = 0.0f;
        }
        if (cTopMarker.transform.position.y + movement.y > mTopLimit.transform.position.y)
        {
            movement.y = 0.0f;
        }
        if (cBottomMarker.transform.position.y + movement.y < mBottomLimit.transform.position.y)
        {
            movement.y = 0.0f;
        }


        return movement;
    }

    public override void button_held_action(CustomInput myInput)
    {
        if (myInput.deltaPosition.magnitude > 0)
        {
            mapDisplayCamera.transform.Translate(
                check_valid_map_movement(myInput.deltaPosition.normalized)
                * Time.deltaTime * cameraMoveSpeed);
        }

        //reposition camera
        //reposition camera
        if (cLeftMarker.transform.position.x < mLeftLimit.transform.position.x)
        {
            Vector3 movementVector = mLeftLimit.transform.position - cLeftMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cRightMarker.transform.position.x > mRightLimit.transform.position.x)
        {
            Vector3 movementVector = mRightLimit.transform.position - cRightMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cTopMarker.transform.position.y > mTopLimit.transform.position.y)
        {
            Vector3 movementVector = mTopLimit.transform.position - cTopMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
        if (cBottomMarker.transform.position.y < mBottomLimit.transform.position.y)
        {
            Vector3 movementVector = mBottomLimit.transform.position - cBottomMarker.transform.position;
            mapDisplayCamera.transform.Translate(movementVector.x, movementVector.y, 0.0f);
        }
    }

	// Use this for initialization
	void Start () {
        initialize_camera();
	}
	
	// Update is called once per frame
	void Update () {
        set_camera_marker();
	}
}
