using UnityEngine;
using System.Collections;

public class ChangeStateTextEffect : MonoBehaviour
{
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
    bool playerSwitchedToClose;

    public GameObject[] closeWeaponSystems;
    public GameObject[] farWeaponSystems;

    public AudioSource myAudio;

    public void initialize_state_change_sequence(bool playerState)
    {
        textObject.SetActive(false);
        Color temp = textBackGroundEffect.color;
        temp.a = 0.0f;
        textBackGroundEffect.color = temp;
        textBackGroundEffect.gameObject.SetActive(false);

        leftSlideText.transform.position = leftSlideTextOriginalPos;
        rightSlideText.transform.position = rightSlideTextOriginalPos;

        for (int ctr = 0; ctr < closeWeaponSystems.Length; ctr++)
        {
            closeWeaponSystems[ctr].SetActive(true);
            Vector3 tempVector3 = closeWeaponSystems[ctr].transform.position;
            tempVector3.x = leftSlideTextOriginalPos.x;
            closeWeaponSystems[ctr].transform.position = tempVector3;
        }

        for (int ctr = 0; ctr < farWeaponSystems.Length; ctr++)
        {
            farWeaponSystems[ctr].SetActive(true);
            Vector3 tempVector3 = farWeaponSystems[ctr].transform.position;
            tempVector3.x = rightSlideTextOriginalPos.x;
            farWeaponSystems[ctr].transform.position = tempVector3;
        }

        sequenceActive = true;
        sequenceInitialized = false;
        curPhaseEndTime = Time.time + 0.2f;
        effectSequence = 0;

        playerSwitchedToClose = playerState;
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
        if (Time.time <= curPhaseEndTime - 0.2f)
        {
            Color temp = textBackGroundEffect.color;
            temp.a += 10.0f * Time.deltaTime;
            textBackGroundEffect.color = temp;
        }
        else if (Time.time > curPhaseEndTime - 0.4f && Time.time < curPhaseEndTime - 0.05f)
        {
            Color temp = textBackGroundEffect.color;
            temp.a = 1.0f;
            textBackGroundEffect.color = temp;
        }
        else if (Time.time > curPhaseEndTime - 0.05f)
        {
            Color temp = textBackGroundEffect.color;
            temp.a -= 10.0f * Time.deltaTime;
            textBackGroundEffect.color = temp;
        }
    }



    //Slide first one in
    void slider_effect_stage3()
    {
        if (playerSwitchedToClose == true)
        {
            Vector3 calcPos = closeWeaponSystems[0].transform.position;
            calcPos.x += slideSpeed * Time.deltaTime;
            closeWeaponSystems[0].transform.position = calcPos;
        }
        else
        {
            Vector3 calcPos = farWeaponSystems[0].transform.position;
            calcPos.x -= slideSpeed * Time.deltaTime;
            farWeaponSystems[0].transform.position = calcPos;
        }
    }

    //slide second one in
    void slider_effect_stage4()
    {
        if (playerSwitchedToClose == true)
        {
            Vector3 calcPos = closeWeaponSystems[1].transform.position;
            calcPos.x += slideSpeed * Time.deltaTime;
            closeWeaponSystems[1].transform.position = calcPos;
        }
        else
        {
            Vector3 calcPos = farWeaponSystems[1].transform.position;
            calcPos.x -= slideSpeed * Time.deltaTime;
            farWeaponSystems[1].transform.position = calcPos;
        }
    }

    //Slide first one out
    void slider_effect_stage5()
    {
        if (playerSwitchedToClose == true)
        {
            Vector3 calcPos = closeWeaponSystems[0].transform.position;
            calcPos.x -= slideSpeed * Time.deltaTime;
            closeWeaponSystems[0].transform.position = calcPos;
        }
        else
        {
            Vector3 calcPos = farWeaponSystems[0].transform.position;
            calcPos.x += slideSpeed * Time.deltaTime;
            farWeaponSystems[0].transform.position = calcPos;
        }

    }

    //Slide second one out;
    void slider_effect_stage6()
    {
        if (playerSwitchedToClose == true)
        {
            Vector3 calcPos = closeWeaponSystems[1].transform.position;
            calcPos.x -= slideSpeed * Time.deltaTime;
            closeWeaponSystems[1].transform.position = calcPos;
        }
        else
        {
            Vector3 calcPos = farWeaponSystems[1].transform.position;
            calcPos.x += slideSpeed * Time.deltaTime;
            farWeaponSystems[1].transform.position = calcPos;
        }
    }

    void slider_effect_stage7()
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

        for (int ctr = 0; ctr < closeWeaponSystems.Length; ctr++)
        {
            closeWeaponSystems[ctr].SetActive(false);
        }

        for (int ctr = 0; ctr < farWeaponSystems.Length; ctr++)
        {
            farWeaponSystems[ctr].SetActive(false);
        }

        sequenceActive = false;
    }

    // Use this for initialization
    void Start()
    {
        leftSlideTextOriginalPos = leftSlideText.transform.position;
        rightSlideTextOriginalPos = rightSlideText.transform.position;

        slideSpeed = (rightSlideTextOriginalPos.x - textObject.transform.position.x) * 5.0f;

        textObject.SetActive(false);
        textBackGroundEffect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
                    curPhaseEndTime = Time.time + 0.3f;
                    myAudio.Play();
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
                    curPhaseEndTime = Time.time + 0.2f;
                    if (playerSwitchedToClose == true)
                    {
                        Vector3 calcPos = closeWeaponSystems[0].transform.position;
                        calcPos.x = textObject.transform.position.x;
                        closeWeaponSystems[0].transform.position = calcPos;
                    }
                    else
                    {
                        Vector3 calcPos = farWeaponSystems[0].transform.position;
                        calcPos.x = textObject.transform.position.x;
                        farWeaponSystems[0].transform.position = calcPos;
                    }
                }
            }
            else if (effectSequence == 3)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage4();
                }
                else
                {
                    effectSequence++;
                    sequenceInitialized = false;
                    curPhaseEndTime = Time.time + 0.2f;
                    if (playerSwitchedToClose == true)
                    {
                        Vector3 calcPos = closeWeaponSystems[1].transform.position;
                        calcPos.x = textObject.transform.position.x;
                        closeWeaponSystems[1].transform.position = calcPos;
                    }
                    else
                    {
                        Vector3 calcPos = farWeaponSystems[1].transform.position;
                        calcPos.x = textObject.transform.position.x;
                        farWeaponSystems[1].transform.position = calcPos;
                    }
                }
            }
            else if (effectSequence == 4)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage5();
                }
                else
                {
                    effectSequence++;
                    sequenceInitialized = false;
                    curPhaseEndTime = Time.time + 0.2f;
                }
            }
            else if (effectSequence == 5)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage6();
                }
                else
                {
                    effectSequence++;
                    sequenceInitialized = false;
                    curPhaseEndTime = Time.time + 0.2f;
                }
            }
            else if (effectSequence == 6)
            {
                if (Time.time < curPhaseEndTime)
                {
                    slider_effect_stage7();
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
                gameObject.SetActive(false);
            }
        }
    }
}
