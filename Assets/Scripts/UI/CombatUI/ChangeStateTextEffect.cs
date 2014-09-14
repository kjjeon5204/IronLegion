using UnityEngine;
using System.Collections;

public class ChangeStateTextEffect : MonoBehaviour {
    public GameObject leftSlideText;
    public GameObject rightSlideText;

    Vector3 leftSlideTextOriginalPos;
    Vector3 rightSlideTextOriginalPos;

    public SpriteRenderer textBackGroundEffect;

    public GameObject textObject;

    float timeTracker;
    float curPhaseEndTime;

    float slideSpeed;

    int effectSequence = 0;
    bool sequenceInitialized = false;
    bool sequenceActive = false;

    public void initialize_state_change_sequence()
    {
        textObject.SetActive(false);
        Color temp = textBackGroundEffect.color;
        temp.a = 0.0f;
        textBackGroundEffect.color = temp;
        textBackGroundEffect.gameObject.SetActive(false);

        leftSlideText.transform.position = leftSlideTextOriginalPos;
        rightSlideText.transform.position = rightSlideTextOriginalPos;

        sequenceActive = true;
        sequenceInitialized = false;
        curPhaseEndTime = Time.time + 0.2f;
        effectSequence = 0;
    }

    void slider_effect_stage1()
    {
        Vector3 calcPos = leftSlideText.transform.position;
        calcPos.x += slideSpeed * Time.deltaTime;
        leftSlideText.transform.position = calcPos;
        calcPos = rightSlideText.transform.position;
        calcPos.x -= slideSpeed * Time.deltaTime;
        rightSlideText.transform.position = calcPos;
    }

    void slider_effect_stage2()
    {
        if (sequenceInitialized == false)
        {
            textObject.SetActive(true);
            textBackGroundEffect.gameObject.SetActive(true);

            leftSlideText.transform.position = textObject.transform.position;
            rightSlideText.transform.position = textObject.transform.position;

            sequenceInitialized = true;
        }
        if (Time.time <= curPhaseEndTime - 0.4f)
        {
            Color temp = textBackGroundEffect.color;
            temp.a += 10.0f * Time.deltaTime;
            textBackGroundEffect.color = temp;
        }
        else if (Time.time > curPhaseEndTime - 0.4f && Time.time < curPhaseEndTime - 0.1f)
        {
            Color temp = textBackGroundEffect.color;
            temp.a = 1.0f;
            textBackGroundEffect.color = temp;
        }
        else if (Time.time > curPhaseEndTime - 0.1f)
        {
            Color temp = textBackGroundEffect.color;
            temp.a -= 10.0f * Time.deltaTime;
            textBackGroundEffect.color = temp;
        }
    }

    void slider_effect_stage3()
    {
        if (sequenceInitialized == false)
        {
            textObject.SetActive(false);
            textBackGroundEffect.gameObject.SetActive(false);

            sequenceInitialized = true;
        }
        Vector3 calcPos = leftSlideText.transform.position;
        calcPos.x -= slideSpeed * Time.deltaTime;
        leftSlideText.transform.position = calcPos;
        calcPos = rightSlideText.transform.position;
        calcPos.x += slideSpeed * Time.deltaTime;
        rightSlideText.transform.position = calcPos;
    }

    void end_sequence()
    {
        textObject.SetActive(false);
        textBackGroundEffect.gameObject.SetActive(false);

        sequenceActive = false;
    }

	// Use this for initialization
	void Start () {
        leftSlideTextOriginalPos = leftSlideText.transform.position;
        rightSlideTextOriginalPos = rightSlideText.transform.position;

        slideSpeed = (rightSlideTextOriginalPos.x - textObject.transform.position.x) * 5.0f;

        textObject.SetActive(false);
        textBackGroundEffect.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (sequenceActive == true)
        {
            if (effectSequence == 0)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage1();
                }
                else
                {
                    effectSequence++;
                    sequenceInitialized = false;
                    curPhaseEndTime = Time.time + 0.5f;
                }
            }
            else if (effectSequence == 1)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage2();
                }
                else
                {
                    effectSequence++;
                    sequenceInitialized = false;
                    curPhaseEndTime = Time.time + 0.2f;
                }
            }
            else if (effectSequence == 2)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage3();
                }
                else
                {
                    effectSequence++;
                    sequenceInitialized = false;
                }
            }
            else
            {
                end_sequence();
            }
        }
	}
}
