using UnityEngine;
using System.Collections;

public class UIResizeScript : MonoBehaviour {
    public Camera resizeToCam;
    public Rect uiSize;
    SpriteRenderer curSpriteRenderer;


    void texture_resize(GameObject target, Rect targetSize)
    {
        SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = resizeToCam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = resizeToCam.WorldToViewportPoint(targetSprite.bounds.min);
        Vector3 xMax = resizeToCam.WorldToViewportPoint(targetSprite.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
        //Debug.Log(scaleFactor);
    }

	// Use this for initialization
	void Start () {
        texture_resize(gameObject, uiSize);   
	}
}
