using UnityEngine;
using System.Collections;


public class UI2DTransitionEffect : MonoBehaviour {
    public SpriteRenderer curSpriteToModify;
    public float transitionLength;
    

    public SpriteRenderer state1SpriteBound;
    public SpriteRenderer state2SpriteBound;
    public int curState = 0;

    public bool inStateTransition = false;
    float timeTracker;

    Vector3 centerShiftRate;
    Vector3 scaleChangeRate;

    Vector3 destinationScale;


    public void initiate_ui_transition()
    {
        Bounds boundsTotalChange = new Bounds();
        if (curState == 0)
        {
            boundsTotalChange.center = state2SpriteBound.bounds.center
                - curSpriteToModify.bounds.center;

            destinationScale = Vector3.zero;
            destinationScale.x = state2SpriteBound.bounds.extents.x /
                curSpriteToModify.bounds.extents.x * curSpriteToModify.transform.localScale.x;
            destinationScale.y = state2SpriteBound.bounds.extents.y /
                curSpriteToModify.bounds.extents.y * curSpriteToModify.transform.localScale.y;

            scaleChangeRate = (destinationScale - 
                curSpriteToModify.transform.localScale) / transitionLength;
            centerShiftRate = boundsTotalChange.center / transitionLength;

        }
        if (curState == 1)
        {
            boundsTotalChange.center = state1SpriteBound.bounds.center
                - curSpriteToModify.bounds.center;

            destinationScale = Vector3.zero;
            destinationScale.x = state1SpriteBound.bounds.extents.x /
                curSpriteToModify.bounds.extents.x * curSpriteToModify.transform.localScale.x;
            destinationScale.y = state1SpriteBound.bounds.extents.y /
                curSpriteToModify.bounds.extents.y * curSpriteToModify.transform.localScale.y;

            scaleChangeRate = (destinationScale -
                curSpriteToModify.transform.localScale) / transitionLength;
            centerShiftRate = boundsTotalChange.center / transitionLength;
        }
        timeTracker = 0.0f;
        inStateTransition = true;
    }

	// Use this for initialization
	void Start () {
        if (inStateTransition == true)
            initiate_ui_transition();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            initiate_ui_transition();
        }
        if (inStateTransition == true)
        {
            curSpriteToModify.transform.localScale += scaleChangeRate * Time.deltaTime;
            curSpriteToModify.transform.Translate(centerShiftRate * Time.deltaTime);
            if (curState == 0)
            {
                if (timeTracker > transitionLength)
                {
                    curSpriteToModify.transform.localScale = destinationScale;
                    curSpriteToModify.transform.position = state2SpriteBound.bounds.center;
                    curState = 1;
                    inStateTransition = false;
                }
            }
            else if (curState == 1)
            {
                if (timeTracker > transitionLength)
                {
                    curSpriteToModify.transform.localScale = destinationScale;
                    curSpriteToModify.transform.position = state1SpriteBound.bounds.center;
                    curState = 0;
                    inStateTransition = false;
                }
            }
            timeTracker += Time.deltaTime;
        }
	}
}
