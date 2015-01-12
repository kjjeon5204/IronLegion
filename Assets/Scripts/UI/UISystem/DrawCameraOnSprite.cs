using UnityEngine;
using System.Collections;

public class DrawCameraOnSprite : MonoBehaviour {
    public SpriteRenderer drawBox;
    Bounds drawBounds;
    public Camera fullViewportCam;
    Camera myCam;
    bool calcCam = false;

    Rect convert_normalized_coord()
    {
        
        Rect convertedRect;
        drawBounds = drawBox.bounds;
        Vector3 center = drawBounds.center;
        Vector3 lowerLeftNormPt = fullViewportCam.WorldToViewportPoint(drawBounds.center - drawBounds.extents);
        Vector3 upperRightNormPt = fullViewportCam.WorldToViewportPoint(drawBounds.center + drawBounds.extents);


        convertedRect = new Rect(lowerLeftNormPt.x, lowerLeftNormPt.y,
            upperRightNormPt.x - lowerLeftNormPt.x, upperRightNormPt.y - lowerLeftNormPt.y);
        if (calcCam == false) 
        {
            Debug.Log("Converted Rect: " + convertedRect);
            calcCam = true;
        }
        return convertedRect;
         
    }

    float spriteAspectRatio;
    void get_sprite_aspect_ratio()
    {
        spriteAspectRatio = drawBox.bounds.extents.x / drawBox.bounds.extents.y;

    }

    void set_camera_size()
    {
        myCam.orthographicSize = drawBox.bounds.extents.y;
    }


    void Start()
    {
        myCam = GetComponent<Camera>();
        if (myCam != null)
        {
            get_sprite_aspect_ratio();
            set_camera_size();
            myCam.rect = convert_normalized_coord();
        }
    }

    void Update()
    {
        if (myCam != null)
        {
            get_sprite_aspect_ratio();
            set_camera_size();
            myCam.rect = convert_normalized_coord();
        }
    }
}
