using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TutorialCutScene : BattleStory
{
    public GameObject cutSceneCamObject;
    Camera cutSceneCam;

    public GameObject cutScenePersCam;
    public GameObject playerMainCam;


    public GameObject cutSceneDialogueBoxObject;
    public GameObject cutSceneCharacterTextObject;
    TextMesh cutSceneCharacterText;
    public GameObject cutSceneDialogueTextObject;
    TextMesh cutSceneDialogueText;
    public GameObject cutScenePortraitObject;
    SpriteRenderer cutScenePortrait;
    public GameObject cutSceneDialogueObject;
    CombatDialogue cutSceneDialogue;

    public GameObject lowerRightFrame;
    public GameObject skillButtons;
    public GameObject stateChangeText;
    public GameObject[] abilityButtons;
    UIStringModifier stateChangeTextMod;
    public GameObject[] closeSkillSlots;
    public GameObject[] rangeSkillSlots;

    public GameObject lowerLeftFrame;
    public GameObject changeTargetButton;
    public GameObject changeTargetText;

    public GameObject hpBar;
    public GameObject playerHpBar;
    public GameObject[] energyBar;

    public GameObject enemyDisplay;
    public GameObject enemyDistplayText;
    public GameObject enemyHealthBar;


    public MainChar playerScript;
    public GameObject battleBoundary;
    public GameObject eventControls;
    public Radar radarScript;
    public Character[] enemies;

    public GameObject horizontalFingerSlide;

    public GameObject verticalFingerSlide;

    public ShockWaveControl[] skillButtonControls;
    public ShockWaveControl regularAttackButton;

    UIStringModifier textModifier;

    int dialogueCtr;
    int tutorialPhase = 0;
    bool uiModified = false;
    bool conditionMet = false;

    Vector2 curRecord;


    void texture_resize(GameObject target, Rect targetSize)
    {
        SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = cutSceneCam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = cutSceneCam.WorldToViewportPoint(targetSprite.bounds.min);
        Vector3 xMax = cutSceneCam.WorldToViewportPoint(targetSprite.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
        //Debug.Log(scaleFactor);
    }

    public void initialize_buttons()
    {
        AbilityButton buttonAccess;
        for (int ctr = 0; ctr < 4; ctr++)
        {
            buttonAccess = closeSkillSlots[ctr].GetComponent<AbilityButton>();
            buttonAccess.eventControlObject = eventControls;
            if (playerScript.abilityNames[ctr] != null)
            {
                float coolDown = playerScript.abilityDictionary[playerScript.abilityNames[ctr]].myData.cooldown;
                if (playerScript.abilityDictionary[playerScript.abilityNames[ctr]].myData.startCooldown)
                {
                    buttonAccess.initialize_button(playerScript.abilityNames[ctr], coolDown, coolDown);
                }
                else
                {
                    buttonAccess.initialize_button(playerScript.abilityNames[ctr], coolDown, 0.0f);
                }
            }
            else
            {
                buttonAccess.initialize_button(null, 0.0f, 0.0f);
            }
        }
        for (int ctr = 4; ctr < 8; ctr++)
        {
            buttonAccess = rangeSkillSlots[ctr - 4].GetComponent<AbilityButton>();
            buttonAccess.eventControlObject = eventControls;
            if (playerScript.abilityNames[ctr] != null)
            {
                float coolDown = playerScript.abilityDictionary[playerScript.abilityNames[ctr]].myData.cooldown;
                if (playerScript.abilityDictionary[playerScript.abilityNames[ctr]].myData.startCooldown)
                {
                    buttonAccess.initialize_button(playerScript.abilityNames[ctr], coolDown, coolDown);
                }
                else
                {
                    buttonAccess.initialize_button(playerScript.abilityNames[ctr], coolDown, 0.0f);
                }
            }
            else
            {
                buttonAccess.initialize_button(null, 0.0f, 0.0f);
            }
        }
    }


    // Use this for initialization
    public override void manual_start()
    {
        base.manual_start();
        cutSceneCam = cutSceneCamObject.GetComponent<Camera>();
        Rect dialogueBoxSize = new Rect(0.1f, 0.6f, 0.8f, 0.3f);
        texture_resize(cutSceneDialogueBoxObject, dialogueBoxSize);

        Rect uiButtonSize = new Rect(0.7f, 0.55f, 0.3f, 0.45f);
        texture_resize(lowerRightFrame, uiButtonSize);
        stateChangeTextMod = stateChangeText.GetComponent<UIStringModifier>();
        stateChangeTextMod.initialize_text("Blade\nAttack");

        uiButtonSize = new Rect(0.0f, 0.55f, 0.3f, 0.45f);
        texture_resize(lowerLeftFrame, uiButtonSize);
        textModifier = changeTargetText.GetComponent<UIStringModifier>();
        textModifier.initialize_text("Change\nTarget");

        uiButtonSize = new Rect(0.37f, 0.0f, 0.3f, 0.15f);
        texture_resize(enemyDisplay, uiButtonSize);
        textModifier = enemyDistplayText.GetComponent<UIStringModifier>();


        uiButtonSize = new Rect(0.0f, 0.0f, 0.3f, 0.15f);
        texture_resize(hpBar, uiButtonSize);
        /*
        uiButtonSize = new Rect(0.4f, 0.2f, 0.2f, 0.35f);
        texture_resize(verticalFingerSlide, uiButtonSize);

        uiButtonSize = new Rect(0.3f, 0.3f, 0.4f, 0.15f);
        texture_resize(horizontalFingerSlide, uiButtonSize);
        */
        horizontalFingerSlide.SetActive(false);
        verticalFingerSlide.SetActive(false);

        //get dialogue text mesh
        cutSceneDialogueText = cutSceneDialogueTextObject.GetComponent<TextMesh>();

        //get character text mesh
        cutSceneCharacterText = cutSceneCharacterTextObject.GetComponent<TextMesh>();

        //Get portrait object
        cutScenePortrait = cutScenePortraitObject.GetComponent<SpriteRenderer>();

        //Get dialogue data
        cutSceneDialogue = cutSceneDialogueObject.GetComponent<CombatDialogue>();
        dialogueCtr = 0;
        get_next_text();
        tutorialPhase = 0;
        
        playerScript.battleBoundary = battleBoundary.collider;
        playerScript.worldObject = eventControls;
        playerScript.set_battle_type(BattleType.REGULAR);
        playerScript.manual_start();
        playerScript.enemyList = enemies;
        foreach (Character enemy in enemies)
        {
            enemy.set_level(1);
            enemy.manual_start();
        }

        initialize_buttons();

    }

    void disable_combat_ui()
    {
        lowerRightFrame.SetActive(false);
        lowerLeftFrame.SetActive(false);
        enemyDisplay.SetActive(false);
        //hpBar.SetActive(false);
    }

    void enable_combat_ui()
    {
        lowerRightFrame.SetActive(true);
        lowerLeftFrame.SetActive(true);
        enemyDisplay.SetActive(true);
        hpBar.SetActive(true);
    }

    void disable_close_buttons()
    {
        foreach (GameObject skillButton in closeSkillSlots)
        {
            skillButton.SetActive(false);
        }
    }

    void enable_close_buttons()
    {
        foreach (GameObject skillButton in closeSkillSlots)
        {
            skillButton.SetActive(true);
        }
    }

    void disable_ranged_buttons()
    {
        foreach (GameObject skillButton in rangeSkillSlots)
        {
            skillButton.SetActive(false);
        }
    }

    void enable_ranged_buttons() {
        foreach (GameObject skillButton in rangeSkillSlots)
        {
            skillButton.SetActive(true);
        }
    }

    void combat_stage1_enable()
    {
        disable_combat_ui();
        lowerRightFrame.SetActive(true);
        skillButtons.SetActive(true);
        disable_close_buttons();
        disable_ranged_buttons();
        
    }

    void combat_stage2_enable()
    {
        disable_combat_ui();
        lowerRightFrame.SetActive(true);
        skillButtons.SetActive(true);
        enable_close_buttons();
        disable_ranged_buttons();
    }

    void combat_stage3_enable()
    {
        disable_combat_ui();
        lowerRightFrame.SetActive(true);
        lowerLeftFrame.SetActive(true);
    }

    void disable_dialogue()
    {
        cutSceneDialogueBoxObject.SetActive(false);
    }

    void enable_dialogue()
    {
        cutSceneDialogueBoxObject.SetActive(true);
    }



    void tutorial_input(Touch acc)
    {
        int layerMask = 1 << 14;
        Vector3 touchPos = cutSceneCam.ScreenToWorldPoint(acc.position);
        RaycastHit2D hitButton = Physics2D.Raycast(touchPos, Vector3.forward, 100.0f, layerMask);
        if (hitButton.collider != null)
        {
            if (hitButton.collider.gameObject == skillButtons && playerScript.curState == "IDLE"
                 && conditionMet == false)
            {
                if (playerScript.isClose == true)
                {
                    if (playerScript.regAttackCtr == 0 && playerScript.curState != "REGULAR_ATTACK1")
                    {
                        if (playerScript.abilityDictionary["REGULAR_ATTACK1"].initialize_ability())
                        {
                            playerScript.curState = "REGULAR_ATTACK1";
                        }
                    }
                    else if (playerScript.regAttackCtr == 1 && playerScript.isClose == true)
                    {
                        if (playerScript.abilityDictionary["REGULAR_ATTACK2"].initialize_ability())
                        {
                            playerScript.curState = "REGULAR_ATTACK2";
                            playerScript.regAttackCtr = 0;
                        }
                    }
                }
                if (playerScript.isClose == false)
                {
                    if (playerScript.abilityDictionary["SHOULDER_GUN_ATTACK"].initialize_ability())
                    {
                        playerScript.curState = "SHOULDER_GUN_ATTACK";
                    }
                }
                conditionMet = true;
            }
            else if (hitButton.collider.tag == "AbilityButton" && playerScript.curState == "IDLE" &&
                tutorialPhase == 4 && conditionMet == false)
            {
                playerScript.turn_off_effect();
                AbilityButton pressAbilityButton = hitButton.collider.gameObject.GetComponent<AbilityButton>();
                if (pressAbilityButton.is_button_ready() && playerScript.player_input_ready())
                {
                    string skillName = pressAbilityButton.button_pressed();
                    playerScript.curState = skillName;
                    playerScript.abilityDictionary[skillName].initialize_ability();
                }
                conditionMet = true;
            }
        }
        else
        {

            Debug.Log("Attack triggered");
            curRecord += acc.deltaPosition;
            if (acc.phase == TouchPhase.Ended && curRecord != Vector2.zero && playerScript.curState == "IDLE")
            {
                if (Mathf.Abs(curRecord.x) > Mathf.Abs(curRecord.y) && tutorialPhase == 8)
                {
                    if (curRecord.x > 0.0f)
                    {
                        if (playerScript.abilityDictionary["DODGE_RIGHT"].initialize_ability())
                        {
                            playerScript.curState = "DODGE_RIGHT";
                            playerScript.LexhaustScript.instant_thruster(3.0f);
                            playerScript.lKneeExhaustScript.instant_thruster(3.5f);
                            playerScript.lLegExhaustScript.instant_thruster(3.5f);
                        }
                    }
                    if (curRecord.x < 0.0f)
                    {
                        if (playerScript.abilityDictionary["DODGE_LEFT"].initialize_ability())
                        {
                            playerScript.curState = "DODGE_LEFT";
                            playerScript.RexhaustScript.instant_thruster(3.0f);
                            playerScript.rKneeExhaustScript.instant_thruster(3.5f);
                            playerScript.rLegExhaustScript.instant_thruster(3.5f);
                        }
                    }
                    conditionMet = true;
                }

                else if (playerScript.curState == "IDLE" && tutorialPhase == 6)
                {
                    playerScript.switch_hero_state();
                    playerScript.curEnergy -= 10.0f;
                    if (playerScript.isClose == true && curRecord.y < 0.0f)
                    {
                        stateChangeTextMod.initialize_text("Phaser\nAttack");
                        enable_ranged_buttons();
                        disable_close_buttons();
                        playerScript.isClose = false;
                        playerScript.switch_hero_state();
                    }
                    else if (playerScript.isClose == false && curRecord.y > 0.0f)
                    {

                        stateChangeTextMod.initialize_text("Blade\nAttack");
                        enable_close_buttons();
                        disable_ranged_buttons();
                        playerScript.isClose = true;

                        playerScript.switch_hero_state();
                    }
                    conditionMet = true;
                }
            }
        }
    }

    //true successful
    //false no dialogue remaining
    bool get_next_text()
    {
        if (dialogueCtr == cutSceneDialogue.dialogues.Length)
        {
            return false;
        }
        else
        {
            cutScenePortrait.sprite = cutSceneDialogue.dialogues[dialogueCtr].characterSprite;
            cutSceneDialogueText.text = cutSceneDialogue.dialogues[dialogueCtr].text;
            cutSceneCharacterText.text = cutSceneDialogue.dialogues[dialogueCtr].characterName;
            dialogueCtr++;
            return true;
        }

    }

    bool tutorial_last_phase()
    {
        for (int ctr = 0; ctr < enemies.Length; ctr++)
        {
            if (enemies[ctr] != null)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    public override bool manual_update()
    {
        //PC
        //update player data
        float playerHP = 1.0f * playerScript.return_cur_stats().baseHp / playerScript.return_base_stats().baseHp;
        if (playerHP < 0.0f)
        {
            playerHP = 0.0f;
        }
        playerHpBar.transform.localScale = new Vector3(playerHP, 1.0f, 1.0f);
        int bar = (int)(playerScript.energyPercentage * energyBar.Length);
        for (int ctr = 0; ctr < energyBar.Length; ctr++)
        {
            if (ctr < bar)
                energyBar[ctr].SetActive(true);
            else energyBar[ctr].SetActive(false);
        }

        //Initial dialogue
        if (tutorialPhase == 0)
        {
            if (uiModified == false)
            {
                playerMainCam.SetActive(false);
                disable_combat_ui();
                uiModified = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                get_next_text();
                if (dialogueCtr == 14)
                {
                    tutorialPhase++;
                    uiModified = false;
                }
            }
        }
        //Regular attack
        else if (tutorialPhase == 1)
        {
            if (uiModified == false)
            {
                playerMainCam.SetActive(true);
                cutScenePersCam.SetActive(false);
                uiModified = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                get_next_text();
                if (dialogueCtr == 15)
                {
                    tutorialPhase++;
                    uiModified = false;
                }
            }
        }
        //Regular player input
        else if (tutorialPhase == 2)
        {
            playerScript.manual_update();

            //regular attack tutorial
            if (uiModified == false)
            {
                uiModified = true;
                combat_stage1_enable();
                disable_dialogue();
                conditionMet = false;
                regularAttackButton.activate_button();
            }
            
            if (Input.touchCount > 0)
                tutorial_input(Input.GetTouch(0));
            
            if (Input.GetKeyDown(KeyCode.M))
                conditionMet = true;

            if (conditionMet == true && playerScript.curState == "IDLE")
            {
                tutorialPhase++;
                uiModified = false;
                regularAttackButton.deactivate_button_shock();
            }
        }
        //Ability
        else if (tutorialPhase == 3)
        {
            if (uiModified == false)
            {
                disable_combat_ui();
                enable_dialogue();
                uiModified = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                get_next_text();
                if (dialogueCtr == 19)
                {
                    tutorialPhase++;
                    uiModified = false;
                }
            }
        }
        //ability input
        else if (tutorialPhase == 4)
        {
            playerScript.manual_update();
            //regular attack tutorial
            if (uiModified == false)
            {
                uiModified = true;
                combat_stage2_enable();
                disable_dialogue();
                conditionMet = false;
                foreach (ShockWaveControl myControl in skillButtonControls)
                {
                    myControl.activate_button();
                }
            }

            if (Input.touchCount > 0)
                tutorial_input(Input.GetTouch(0));

            if (Input.GetKeyDown(KeyCode.M))
                conditionMet = true;

            if (conditionMet == true && playerScript.curState == "IDLE")
            {
                tutorialPhase++;
                uiModified = false;
                foreach (ShockWaveControl myControl in skillButtonControls)
                {
                    myControl.deactivate_button_shock();
                }
            }
        }
        //Change State
        else if (tutorialPhase == 5)
        {
            if (uiModified == false)
            {
                disable_combat_ui();
                enable_dialogue();
                uiModified = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                get_next_text();
                if (dialogueCtr == 23)
                {
                    tutorialPhase++;
                    uiModified = false; 
                }
            }
        }
        //Change state input
        else if (tutorialPhase == 6)
        {
            playerScript.manual_update();
            //regular attack tutorial
            if (uiModified == false)
            {
                verticalFingerSlide.SetActive(true);
                uiModified = true;
                combat_stage3_enable();
                disable_dialogue();
                conditionMet = false;
            }

            if (Input.touchCount > 0)
                tutorial_input(Input.GetTouch(0));

            if (Input.GetKeyDown(KeyCode.M))
                conditionMet = true;

            if (conditionMet == true && playerScript.curState == "IDLE")
            {
                verticalFingerSlide.SetActive(false);
                tutorialPhase++;
                uiModified = false;
            }
        }
        //Dodge dialogue
        else if (tutorialPhase == 7)
        {
            if (uiModified == false)
            {
                disable_combat_ui();
                enable_dialogue();
                uiModified = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                get_next_text();
                if (dialogueCtr == 30)
                {
                    tutorialPhase++;
                    uiModified = false;
                }
            }
        }
        //Dodge Dialogue input
        else if (tutorialPhase == 8)
        {
            playerScript.manual_update();
            //regular attack tutorial
            if (uiModified == false)
            {
                horizontalFingerSlide.SetActive(true);
                uiModified = true;
                combat_stage1_enable();
                disable_dialogue();
                conditionMet = false;
            }

            if (Input.touchCount > 0)
                tutorial_input(Input.GetTouch(0));

            if (Input.GetKeyDown(KeyCode.M))
                conditionMet = true;

            if (conditionMet == true && playerScript.curState == "IDLE")
            {
                horizontalFingerSlide.SetActive(false);
                tutorialPhase++;
                uiModified = false;
            }
        }
        //briefing
        else if (tutorialPhase == 9)
        {
            if (uiModified == false)
            {
                disable_combat_ui();
                enable_dialogue();
                uiModified = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                if (!get_next_text())
                {
                    tutorialPhase++;
                    uiModified = false;
                }
            }
        }
        else
        {
            Debug.Log("Tutorial Phase " + tutorialPhase);
            return true;
        }
        return false;
    }
}