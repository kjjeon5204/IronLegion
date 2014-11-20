using UnityEngine;
using System.Collections;

public class DrawCameraOnSprite : MonoBehaviour {
    public SpriteRenderer drawBox;
    Bounds drawBounds;
    public Camera fullViewportCam;
    Camera myCam;

    Rect convert_normalized_coord()
    {
        Rect convertedRect;
        drawBounds = drawBox.bounds;
        Vector3 center = drawBounds.center;
        Vector3 lowerLeftNormPt = fullViewportCam.WorldToScreenPoint(drawBounds.center - drawBounds.extents);
        Vector3 upperRightNormPt = fullViewportCam.WorldToScreenPoint(drawBounds.center + drawBounds.extents);

        //Debug.Log("lower left normalized point: " + lowerLeftNormPt);
        //Debug.Log("upper right normalized point: " + upperRightNormPt);

        convertedRect = new Rect(lowerLeftNormPt.x, lowerLeftNormPt.y,
            upperRightNormPt.x - lowerLeftNormPt.x, upperRightNormPt.y - lowerLeftNormPt.y);

        //Debug.Log("Converted Rect: " + convertedRect);
        return convertedRect;
    }


	

    void Update()
    {
        
        myCam = GetComponent<Camera>();
        if (myCam != null)
        {
            myCam.pixelRect = convert_normalized_coord();
        }
    }
}
