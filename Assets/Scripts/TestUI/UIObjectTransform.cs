using UnityEngine;
using System.Collections;

public class UIObjectTransform : MonoBehaviour {
    bool onScreen;
    float distOnToOff;
    float remainingMovement;
    Vector3 movementDirection;

    public void switch_position() {
        if (onScreen == true)
        {
            remainingMovement = distOnToOff;
            movementDirection = Vector3.right;
            onScreen = false;
            Debug.Log("Move Off");
        }
        else if (onScreen == false)
        {
            remainingMovement = distOnToOff;
            movementDirection = Vector3.right;
            onScreen = true;
            Debug.Log("Move In");
        }
    }


    public void set_inital_condition(float inDistOnToOff, bool inOnScreen)
    {
        distOnToOff = inDistOnToOff;
        onScreen = inOnScreen;
    }


	// Use this for initialization
	void Start () {
        remainingMovement = 0.0f;
	}

	
	// Update is called once per frame
	void Update () {
        if (remainingMovement > 0.0f)
        {
            if (onScreen == false) 
            {
                transform.Translate(distOnToOff * movementDirection * Time.deltaTime);
                remainingMovement -= distOnToOff * Time.deltaTime;
                Debug.Log("moving");
                if (remainingMovement < 0.0f)
                {
                    transform.Translate(remainingMovement * movementDirection);
                }
            }
            if (onScreen == true) 
            {
                transform.Translate(distOnToOff * -movementDirection * Time.deltaTime);
                remainingMovement -= distOnToOff * Time.deltaTime;
                Debug.Log("moving");
                if (remainingMovement < 0.0f)
                {
                    transform.Translate(remainingMovement * movementDirection);
                }
            }
        }
	}
}
