using UnityEngine;
using System.Collections;

public class MyPathing : MonoBehaviour {

    public GameObject objectPathUse;
    public Character objectPathUseScript;

    


    public GameObject[] flagList;
    //0 = startFlag
    //n - 1 = last flag


    int flagAcc;
    bool flagDirection;
    //true = 0 -> n
    //false = n -> 0

    public void initialize_path(GameObject usingCharacter)
    {
        objectPathUseScript = usingCharacter.GetComponent<Character>();
		flagAcc = 1;
    }

    public void start_path(GameObject currentFlag)
    {
        if (currentFlag == flagList[0])
        {
            flagDirection = true;
        }
        if (currentFlag == flagList[flagList.Length - 1])
        {
            flagDirection = false;
        }
    }

    public bool run_path()
    {
        if ((objectPathUseScript.transform.position - 
            flagList[flagAcc].transform.position).magnitude < 6.0f)
        {
             flagAcc++;
		}
		if (flagAcc == flagList.Length) {
			Debug.Log ("Pathing complete!");
			return true;
		}

		objectPathUseScript.animation.Play ("move");
        Vector3 flagPos = objectPathUseScript.transform.position - flagList[flagAcc].transform.position;
        if (objectPathUseScript.custom_look_at_3D(flagList[flagAcc].transform.position))
        {
            if (flagPos.y > 5.0f)
            {
                Vector3 movementVector = objectPathUseScript.transform.InverseTransformDirection(Vector3.up);
                objectPathUseScript.transform.Translate(movementVector * 10.0f * Time.deltaTime);
            }
            else
            {
                objectPathUseScript.transform.Translate(20.0f * Vector3.forward * Time.deltaTime);
            }
        }
        return false;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
