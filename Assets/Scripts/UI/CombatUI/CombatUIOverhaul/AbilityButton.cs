﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AbilityButtonData
{
    public string skillName;
    public string skillDescription;

    //For ability not on CD
    public Sprite buttonUp;

    //For ability on CD;
    public Sprite buttonDown;
    public float maxCoolDown;
    public float startingCoolDown;
}

public class AbilityButton : BaseUIButton {
    public AbilityButtonData[] abilityCollection;

    IDictionary<string, AbilityButtonData> abilityButtonLibrary = new Dictionary<string, AbilityButtonData>();

    AbilityButtonData thisButtonInfo;
    SpriteRenderer curRenderer;

    public float maxCoolDown;
    public float curCoolDown;

    //Skill description
    public GameObject skillDescriptionBox;
    public GameObject skillText;

    //Event Control
    public GameObject eventControlObject;
    EventControls eventControlScript;

    //Player
    MainChar mainPlayer;


    public ShockWaveControl touchDetector;


    public GameObject coolDownBar;
    Quaternion initialRotation;

    float timeDisabled;
    bool noAbility = true;

    void OnDisable()
    {
        timeDisabled = Time.time;
    }

    void OnEnable()
    {
        if (thisButtonInfo != null)
        {
            curCoolDown -= Time.time - timeDisabled;
            if (curCoolDown <= 0.0f)
            {
                curCoolDown = 0.0f;
                curRenderer.sprite = thisButtonInfo.buttonUp;
            }
        }
    }



    public void initialize_button(string abilityName, float inMaxCoolDown, float inCurCoolDown)
    {
        TextMesh textAcc = skillText.GetComponent<TextMesh>();
        if (abilityName != null && abilityName != "NONE")
        {
            
            initialRotation = coolDownBar.transform.rotation;
            foreach (AbilityButtonData thisButton in abilityCollection)
            {
                abilityButtonLibrary[thisButton.skillName] = thisButton;
            }
            curRenderer = GetComponent<SpriteRenderer>();
            thisButtonInfo = abilityButtonLibrary[abilityName];
            if (inCurCoolDown == 0.0f)
                curRenderer.sprite = thisButtonInfo.buttonUp;
            else
                curRenderer.sprite = thisButtonInfo.buttonDown;

            textAcc.text = thisButtonInfo.skillDescription;
            maxCoolDown = inMaxCoolDown;
            curCoolDown = inCurCoolDown;
            noAbility = false;
        }
        else
        {
            //textAcc.text = "No Ability";
            skillDescriptionBox.SetActive(false);
            coolDownBar.SetActive(false);
            noAbility = true;
        }
        eventControlScript = eventControlObject.GetComponent<EventControls>();
        mainPlayer = eventControlScript.playerScript;
    }

    public void initialize_button(string abilityName, MainChar mainCharInput)
    {
        TextMesh textAcc = skillText.GetComponent<TextMesh>();
        if (abilityName != null && abilityName != "NONE")
        {

            initialRotation = coolDownBar.transform.rotation;
            foreach (AbilityButtonData thisButton in abilityCollection)
            {
                abilityButtonLibrary[thisButton.skillName] = thisButton;
            }
            curRenderer = GetComponent<SpriteRenderer>();
            thisButtonInfo = abilityButtonLibrary[abilityName];
            if (thisButtonInfo.startingCoolDown == 0.0f)
                curRenderer.sprite = thisButtonInfo.buttonUp;
            else
                curRenderer.sprite = thisButtonInfo.buttonDown;

            textAcc.text = thisButtonInfo.skillDescription;
            maxCoolDown = thisButtonInfo.maxCoolDown;
            curCoolDown = thisButtonInfo.startingCoolDown;
            noAbility = false;
        }
        else
        {
            //textAcc.text = "No Ability";
            skillDescriptionBox.SetActive(false);
            coolDownBar.SetActive(false);
            noAbility = true;
        }
        mainPlayer = mainCharInput;
    }

    public bool is_button_ready()
    {
        if (curCoolDown <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public void enable_shock_wave()
    {
        if (noAbility == false)
            touchDetector.activate_button();
    }

    public string button_pressed()
    {
        mainPlayer.curState = thisButtonInfo.skillName;
        mainPlayer.abilityDictionary[thisButtonInfo.skillName].initialize_ability();
        curRenderer.sprite = thisButtonInfo.buttonDown;
        curCoolDown = maxCoolDown;
        if (touchDetector != null)
            touchDetector.activate_button();
        return thisButtonInfo.skillName;
    }
    
	
	
	// Update is called once per frame
	void Update () {
        if (noAbility == false)
        {
            if (curCoolDown > 0.0f)
            { 
                curCoolDown -= Time.deltaTime;
                if (curCoolDown <= 0.0f)
                {
                    curCoolDown = 0.0f;
                    curRenderer.sprite = thisButtonInfo.buttonUp;
                }
            }
            coolDownBar.transform.rotation = initialRotation;
            coolDownBar.transform.Rotate(Vector3.back, 180.0f * (curCoolDown / maxCoolDown));
        }
    }

    public override void button_pressed_action()
    {
        mainPlayer.curState = thisButtonInfo.skillName;
        mainPlayer.abilityDictionary[thisButtonInfo.skillName].initialize_ability();
        curRenderer.sprite = thisButtonInfo.buttonDown;
        curCoolDown = maxCoolDown;
        if (touchDetector != null)
            touchDetector.activate_button();
    }
}