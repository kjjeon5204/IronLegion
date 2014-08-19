using UnityEngine;
using System.Collections;


public class ArmoryDataFile : MonoBehaviour {
    System.DateTime curTime;
    string readCurTime;
    string nextResetTime;

	// Use this for initialization
	void Start () {
        
        curTime = System.DateTime.UtcNow;
	}
	
	// Update is called once per frame
	void Update () {
        readCurTime = curTime.ToString();
	}
}
