/*
using UnityEngine;
using System.Collections;

public class DummyCam : MonoBehaviour {
    Character playerScript;
    public float dist;
    Vector3 initialDir;
    public float multiplier;

    bool moveForward = true;
    bool moveForwardstart = false;
    bool moveBackwardstart = false;
    float timer;
	// Use this for initialization
	void Start () {
        GameObject player;
        player = GameObject.Find("heromech(Clone)");
        playerScript = player.GetComponent<Character>();
        initialDir = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 lookOutPt = playerScript.target.collider.bounds.center;
        lookOutPt -= (lookOutPt - transform.position).normalized * dist;
        if (Vector3.Distance(transform.position, playerScript.target.collider.bounds.center) < 10 && moveForwardstart == false && moveForward == true)
        {
            moveForward = false;
            moveForwardstart = true;
            timer = Time.time;
        }

        if (Vector3.Distance(transform.position, playerScript.target.collider.bounds.center) > 10 && moveBackwardstart == false && moveForward == false)
        {
            moveForward = true;
            moveBackwardstart = true;
            timer = Time.time;
        }

        if (moveForwardstart == true)
        {
            transform.Translate(Vector3.left * 2f * Time.deltaTime);
            if (timer + 0.8f < Time.time)
            {
                moveForwardstart = false;
            }
         }

        if (moveBackwardstart == true)
        {
            transform.Translate(Vector3.right * 2f * Time.deltaTime);
            if (timer + 0.8f < Time.time)
            {
                moveBackwardstart = false;
            }
        }
       
        transform.LookAt(lookOutPt);
	}
}
*/