using UnityEngine;
using System.Collections;

public class EndingCredit : MonoBehaviour {
    public GameObject creditText;
    public GameObject creditEndTextPos;
    public GameObject creditEndPos;
    public SpriteRenderer toBeContText;
    public TextMesh thankYouText;
    public float scrollSpeed;

    int creditPhase = 0;
    float timeTracker;
    bool phasePlayed = false;
	
	// Update is called once per frame
	void Update () {
        if (creditPhase == 0)
        {
            if (phasePlayed == false)
            {
                phasePlayed = true;
                timeTracker = Time.time + 0.5f;
            }

            Color curColor = toBeContText.color;
            curColor.a += 2.0f * Time.deltaTime;
            toBeContText.color = curColor;
            

            if (Time.time > timeTracker)
            {
                creditPhase++;
                phasePlayed = false;
            }
        }
        else if (creditPhase == 1)
        {
            if (phasePlayed == false)
            {
                phasePlayed = true;
                timeTracker = Time.time + 1.0f;
            }

            if (Time.time > timeTracker)
            {
                creditPhase++;
                phasePlayed = false;
            }
        }
        else if (creditPhase == 2)
        {
            if (phasePlayed == false)
            {
                phasePlayed = true;
                timeTracker = Time.time + 0.5f;
            }
            Color curColor = toBeContText.color;
            curColor.a -= 2.0f * Time.deltaTime;
            toBeContText.color = curColor;

            if (Time.time > timeTracker)
            {
                creditPhase++;
                phasePlayed = false;
            }
        }
        else if (creditPhase == 3)
        {
            creditText.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
            if (creditEndTextPos.transform.position.y > creditEndPos.transform.position.y)
            {
                creditPhase++;
            }
        }
        else if (creditPhase == 4)
        {
            if (phasePlayed == false)
            {
                phasePlayed = true;
                timeTracker = Time.time + 0.5f;
            }

            Color curColor = thankYouText.color;
            curColor.a += 2.0f * Time.deltaTime;
            thankYouText.color = curColor;


            if (Time.time > timeTracker)
            {
                creditPhase++;
                phasePlayed = false;
            }
        }
        else if (creditPhase == 5)
        {
            if (phasePlayed == false)
            {
                phasePlayed = true;
                timeTracker = Time.time + 1.0f;
            }

            if (Time.time > timeTracker)
            {
                creditPhase++;
                phasePlayed = false;
            }
        }
        else if (creditPhase == 6)
        {
            if (phasePlayed == false)
            {
                phasePlayed = true;
                timeTracker = Time.time + 0.5f;
            }
            Color curColor = thankYouText.color;
            curColor.a -= 2.0f * Time.deltaTime;
            thankYouText.color = curColor;

            if (Time.time > timeTracker)
            {
                creditPhase++;
                phasePlayed = false;
            }
        }
        else
        {
            Application.LoadLevel("Overworld");
        }
	}
}
