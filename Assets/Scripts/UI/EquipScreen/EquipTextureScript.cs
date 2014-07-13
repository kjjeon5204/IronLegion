using UnityEngine;
using System.Collections;

public class EquipTextureScript : MonoBehaviour {
    public GameObject[] equipSlots;

    void texture_resize(SpriteRenderer target, Rect targetSize)
    {
        Vector3 targetPos = new Vector3(targetSize.center.x, targetSize.center.y, 10.0f);
        target.transform.position = Camera.main.ViewportToWorldPoint(targetPos);
        Vector3 xMin = Camera.main.WorldToViewportPoint(target.bounds.min);
        Vector3 xMax = Camera.main.WorldToViewportPoint(target.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
    }



	// Use this for initialization
	void Start () {

        SpriteRenderer acc = equipSlots[0].GetComponent<SpriteRenderer>();
        Vector3 viewPortRect = Camera.main.WorldToViewportPoint(equipSlots[0].transform.position);
        Rect modifier = new Rect(viewPortRect.x - 0.02f, viewPortRect.y + 0.075f / 2.0f, 0.053f, 0.1f);
        texture_resize(acc, modifier);

        acc = equipSlots[1].GetComponent<SpriteRenderer>();
        viewPortRect = Camera.main.WorldToViewportPoint(equipSlots[1].transform.position);
        modifier = new Rect(viewPortRect.x - 0.02f, viewPortRect.y + 0.075f / 2.0f, 0.053f, 0.1f);
        texture_resize(acc, modifier);

        acc = equipSlots[2].GetComponent<SpriteRenderer>();
        viewPortRect = Camera.main.WorldToViewportPoint(equipSlots[2].transform.position);
        modifier = new Rect(viewPortRect.x - 0.02f, viewPortRect.y + 0.075f / 2.0f, 0.053f, 0.1f);
        texture_resize(acc, modifier);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
