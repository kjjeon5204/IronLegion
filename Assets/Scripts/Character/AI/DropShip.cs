using UnityEngine;
using System.Collections;

public class DropShip : MonoBehaviour {
    public Character objectHolding;
    bool landed = false;
    float maxYawl = 0.0f;

    public void position_in_the_air()
    {
        Vector3 tempPosition = objectHolding.transform.position;
        tempPosition.y += 8.0f;
        objectHolding.transform.position = tempPosition;
    }


	// Use this for initialization
	void Start () {
        objectHolding = transform.parent.gameObject.GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
        if (objectHolding != null)
        {
            if (objectHolding.transform.position.y > 0.0f && landed == false)
            {
                objectHolding.transform.Translate(Vector3.down * Time.deltaTime * 5.0f);
            }
            if (landed == false && objectHolding.transform.position.y <= 0.0f)
            {
                Vector3 tempPosition = objectHolding.transform.position;
                tempPosition.y = 0.0f;
                objectHolding.transform.position = tempPosition;
                landed = true;
                transform.parent = null;
                objectHolding.unit_successfully_landed();
            }
        }
        if (landed == true)
        {
            if (maxYawl < 50.0f)
            {
                transform.Rotate(Vector3.right * 60.0f * Time.deltaTime);
                maxYawl += 90.0f * Time.deltaTime;
            }
            transform.Translate(Vector3.up * 3.0f * Time.deltaTime);
        }
        if (landed == true && transform.position.y > 10.0f) {
            Destroy(gameObject);
        }
	}
}
