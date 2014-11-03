using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {
    public GameObject[] enemyList;
    GameObject[] enemyIndicator;
    public GameObject enemyIndicatorIcon;

	IList<GameObject> projectileIndicator;

    public GameObject eventControlObject;
    EventControls eventControlScript;

    public GameObject radarOuterRing;

    public GameObject mainChar;
    public float maxScanDist;

    bool scriptActive = false;



    public void initialize_radar(GameObject[] inEnemyList, GameObject inMainChar)
    {
        //Wipe current radar
        if (enemyIndicator != null)
        {
            for (int ctr = 0; ctr < enemyIndicator.Length; ctr++)
            {
                if (enemyIndicator[ctr] != null)
                    Destroy(enemyIndicator[ctr]);
            }
        }
        scriptActive = true;
        mainChar = inMainChar;
        enemyList = inEnemyList;
        enemyIndicator = new GameObject[enemyList.Length];
        for (int ctr = 0; ctr < enemyList.Length; ctr++)
        {
            enemyIndicator[ctr] = (GameObject)Instantiate(enemyIndicatorIcon, Vector3.zero, Quaternion.identity);
            enemyIndicator[ctr].transform.parent = gameObject.transform;
            enemyIndicator[ctr].transform.localPosition = Vector3.zero;
        }
    }



    void update_radar()
    {
        for (int ctr = 0; ctr < enemyList.Length; ctr++)
        {
            if (enemyList[ctr] == null)
            {
                Destroy(enemyIndicator[ctr]);
            }
            else if ((enemyList[ctr].transform.position - mainChar.transform.position).magnitude > maxScanDist) 
            {
                if (enemyIndicator[ctr].activeInHierarchy == true)
                {
                    enemyIndicator[ctr].SetActive(false);
                }
            }
            else if ((enemyList[ctr].transform.position - mainChar.transform.position).magnitude < maxScanDist)
            {
                if (enemyIndicator[ctr].activeInHierarchy == false)
                {
                    enemyIndicator[ctr].SetActive(true);
                }
                Vector3 relativePos = mainChar.transform.InverseTransformPoint(enemyList[ctr].transform.position);
                relativePos = 1.0f * relativePos / maxScanDist;
                relativePos = relativePos / 2.0f;

                relativePos.y = relativePos.z;
                relativePos.z = 0.0f;
                enemyIndicator[ctr].transform.localPosition = relativePos;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update () {
        radarOuterRing.transform.Rotate(Vector3.forward * Time.deltaTime * 15.0f);
        if (scriptActive == true)
        {
            update_radar();
        }
	}
}
