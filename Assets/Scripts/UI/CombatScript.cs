using UnityEngine;
using System.Collections;
using System.Collections.Generic;





public class CombatScript : MonoBehaviour {
    public struct IconPoolData
    {
        public GameObject icon;
        public bool isUse;
    }

    bool isInitialized = false;
    public GameObject lowerRightFrame;
    public GameObject skillButtons;
    public GameObject stateChangeText;
    UIStringModifier stateChangeTextMod;

    public GameObject lowerLeftFrame;
    public GameObject changeTargetButton;
    public GameObject changeTargetText;

    public GameObject hpBar;
    public GameObject playerHpBar;
    public GameObject[] energyBar;

    public GameObject upperRightFrame;
    public GameObject pauseButton;
    public GameObject pauseButtonText;

    public GameObject enemyDisplay;
    public GameObject enemyDistplayText;
    public GameObject enemyHealthBar;

    public GameObject resetPlayerPos;

    public GameObject radarDisplay;

    public GameObject targetIndicator;

    public GameObject enemyDebuffIconHolder;

	public GameObject endGameWindow;
	public EndBattleLogic endGameScript;

    public ItemPoolData itemPool;

    Camera GUICam;


    public GameObject[] closeSkillSlots;
    public GameObject[] rangeSkillSlots;

    public GameObject eventControlObject;
    EventControls eventControlScript;
    MainChar mainCharacter;

    UIStringModifier textModifier;

    float slideThreshHold = 5.0f;
    float slideSensitivity = 10.0f;
    Vector2 curRecord;

    GameObject main3DCam;

    public GameObject retryButton;
    public GameObject overworldButton;

    bool battleStopped = false;

    public GameObject[] debuffIcons;
    IList<IconPoolData>[] debuffIconPool;
  
    public void initialize_buttons()
    {
        AbilityButton buttonAccess;
        for (int ctr = 0; ctr < 4; ctr++)
        {
            buttonAccess = closeSkillSlots[ctr].GetComponent<AbilityButton>();
            buttonAccess.eventControlObject = eventControlObject;
            float coolDown = mainCharacter.abilityDictionary[mainCharacter.abilityNames[ctr]].myData.cooldown;
            if (mainCharacter.abilityDictionary[mainCharacter.abilityNames[ctr]].myData.startCooldown)
            {
                buttonAccess.initialize_button(mainCharacter.abilityNames[ctr], coolDown, coolDown);
            }
            else
            {
                buttonAccess.initialize_button(mainCharacter.abilityNames[ctr], coolDown, 0.0f);
            }
        }
        for (int ctr = 4; ctr < 8; ctr ++)
        {
            buttonAccess = rangeSkillSlots[ctr - 4].GetComponent<AbilityButton>();
            buttonAccess.eventControlObject = eventControlObject;
            float coolDown = mainCharacter.abilityDictionary[mainCharacter.abilityNames[ctr]].myData.cooldown;
            if (mainCharacter.abilityDictionary[mainCharacter.abilityNames[ctr]].myData.startCooldown)
            {
                buttonAccess.initialize_button(mainCharacter.abilityNames[ctr], coolDown, coolDown);
            }
            else
            {
                buttonAccess.initialize_button(mainCharacter.abilityNames[ctr], coolDown, 0.0f);
            }
        }
    }





   


    //permanent functions
    void disable_ability_button(GameObject[] targetButtons)
    {
        foreach (GameObject skillSlot in targetButtons)
        {
            skillSlot.SetActive(false);
            Debug.Log("Button off");
        }
    }

    void enable_ability_button(GameObject[] targetButtons)
    {
        foreach (GameObject skillSlot in targetButtons)
        {
            skillSlot.SetActive(true);
        }
    }

    

    public void initialize_script()
    {
        
        eventControlScript = eventControlObject.GetComponent<EventControls>();
        mainCharacter = eventControlScript.playerScript;

        initialize_buttons();

        //player state, skill button settings.
        if (mainCharacter.get_player_state())
        {
            enable_ability_button(closeSkillSlots);
            disable_ability_button(rangeSkillSlots);
        }
        main3DCam = GameObject.Find("Main Camera");
        
        isInitialized = true;
    }


    int search_available_icon(IList<IconPoolData> iconList)
    {
        for (int ctr = 1; ctr < iconList.Count; ctr++)
        {
            if (iconList[ctr].isUse == false) {
                return ctr;
            }
        }
        return -1;
    }

    public void turn_off_buff_icon(int buffType, int buffSlot)
    {
        IconPoolData temp = debuffIconPool[buffType][buffSlot];
        temp.isUse = false;
        temp.icon.SetActive(false);
        debuffIconPool[buffType][buffSlot] = temp;
    }

	public void turn_off_combat_ui() {
		lowerLeftFrame.SetActive(false);
		lowerRightFrame.SetActive(false);
		hpBar.SetActive(false);
        radarDisplay.SetActive(false);
		upperRightFrame.SetActive(false);
		enemyDisplay.SetActive (false);
	}

	public void turn_on_combat_ui() {
		lowerLeftFrame.SetActive(true);
		lowerRightFrame.SetActive(true);
        hpBar.SetActive(true);
        radarDisplay.SetActive(true);
		upperRightFrame.SetActive(true);
		enemyDisplay.SetActive (true);
	}

	public void enable_end_battle_window(int creditReceived, PlayerLevelReadData playerData,
        bool battleWon, int itemTier) {
        battleStopped = true;
        turn_off_combat_ui();
        Debug.Log("Activate end game window");
		endGameWindow.SetActive(true);
        endGameScript.initializeData(creditReceived, playerData,
            itemPool.get_item_table(0, itemTier), battleWon);


	}

    public void disable_all_icon(Debuff targetDebuffScript)
    {
        for (int ctr = 0; ctr < targetDebuffScript.numOfActiveDebuff; ctr++)
        {
            int typeAcc = targetDebuffScript.trackDebuff[ctr].buffType;
            int buffSlotAcc = targetDebuffScript.trackDebuff[ctr].buffIconSlot;
            IconPoolData temp = debuffIconPool[typeAcc][buffSlotAcc];
            temp.icon.SetActive(false);
            temp.isUse = false;
            debuffIconPool[typeAcc][buffSlotAcc] = temp;
        }
    }

    void modify_enemy_buff()
    {
        Debuff targetDebuffScript = mainCharacter.target.GetComponent<Debuff>();
        Vector3 position = Vector3.zero;
        for (int ctr = 0; ctr < targetDebuffScript.numOfActiveDebuff; ctr++)
        {
            if (targetDebuffScript.trackDebuff[ctr].buffIconSlot == -1)
            {
                int typeAcc = targetDebuffScript.trackDebuff[ctr].buffType;
                int buffSlotAcc = targetDebuffScript.trackDebuff[ctr].buffIconSlot;
                int openSpot = search_available_icon(debuffIconPool[typeAcc]);
                if (openSpot == -1)
                {
                    Debug.Log("Create Debuff Icon");
                    IconPoolData temp;
                    temp.icon = (GameObject)Instantiate(debuffIconPool[typeAcc][0].icon, Vector3.zero, Quaternion.identity);
                    temp.icon.transform.parent = enemyDebuffIconHolder.transform;
                    temp.icon.transform.localPosition = position;
                    temp.isUse = true;
                    targetDebuffScript.trackDebuff[ctr].buffIconSlot = debuffIconPool[typeAcc].Count;
                    debuffIconPool[typeAcc].Add(temp);
                }
                else
                {
                    targetDebuffScript.trackDebuff[typeAcc].buffIconSlot = openSpot;
                    IconPoolData temp = debuffIconPool[typeAcc][openSpot];
                    temp.icon.SetActive(true);
                    temp.isUse = true;
                    temp.icon.transform.localPosition = position;
                    debuffIconPool[typeAcc][openSpot] = temp;
                }
            }
            else if (targetDebuffScript.trackDebuff[ctr].buffIconSlot != -1)
            {
                //place in correct place

                int typeAcc = targetDebuffScript.trackDebuff[ctr].buffType;
                int buffSlotAcc = targetDebuffScript.trackDebuff[ctr].buffIconSlot;
                if (debuffIconPool[typeAcc][buffSlotAcc].icon.activeInHierarchy == false)
                    debuffIconPool[typeAcc][buffSlotAcc].icon.SetActive(true);
                debuffIconPool[typeAcc][buffSlotAcc].icon.transform.localPosition = position;
            }
            position.x += 3.0f;
        }
    }


    void input_commands(Touch acc)
    {
        AbilityButton pressAbilityButton;
        int layerMask = 1 << 9;
        Vector3 touchPos = GUICam.ScreenToWorldPoint(acc.position);
        RaycastHit2D hitButton = Physics2D.Raycast(touchPos, Vector3.forward, 100.0f, layerMask);
        if (hitButton.collider != null)
        {
            //Combat related input
            if (hitButton.collider.name == skillButtons.name && mainCharacter.player_input_ready())
            {
                if (mainCharacter.isClose == true)
                {/*
                    enable_ability_button(rangeSkillSlots);
                    disable_ability_button(closeSkillSlots);
                    mainCharacter.isClose = false;
                  */
                    if (mainCharacter.regAttackCtr == 0 && mainCharacter.curState != "REGULAR_ATTACK1")
                    {
                        if (mainCharacter.abilityDictionary["REGULAR_ATTACK1"].initialize_ability())
                        {
                            mainCharacter.curState = "REGULAR_ATTACK1";
                        }
                    }
                    else if (mainCharacter.regAttackCtr == 1 && mainCharacter.isClose == true)
                    {
                        if (mainCharacter.abilityDictionary["REGULAR_ATTACK2"].initialize_ability())
                        {
                            mainCharacter.curState = "REGULAR_ATTACK2";
                            mainCharacter.regAttackCtr = 0;
                        }
                    }
                }
                if (mainCharacter.isClose == false)
                {
                    if (mainCharacter.abilityDictionary["SHOULDER_GUN_ATTACK"].initialize_ability()) 
                    {
                        mainCharacter.curState = "SHOULDER_GUN_ATTACK";
                    }
                }
            }
            else if (hitButton.collider.name == resetPlayerPos.name && acc.phase == TouchPhase.Ended)
            {
                mainCharacter.reset_player_pos();
            }
            else if (hitButton.collider.tag == "AbilityButton" && mainCharacter.player_input_ready())
            {
                pressAbilityButton = hitButton.collider.gameObject.GetComponent<AbilityButton>();
                if (pressAbilityButton.is_button_ready() && mainCharacter.player_input_ready())
                {
                    pressAbilityButton.button_pressed();
                }
            }
            else if (hitButton.collider.name == changeTargetButton.name && acc.phase == TouchPhase.Ended
			         && mainCharacter.player_input_ready())
            {
                mainCharacter.get_next_target();
            }
            //*****************************
            //*******End battle Inputs*****
            //*****************************

            else if (hitButton.collider.gameObject == retryButton)
            {
                Application.LoadLevel(2);
            }
            else if (hitButton.collider.gameObject == overworldButton)
            {
                Application.LoadLevel(0);
            }
        }
        else
        {
            curRecord += acc.deltaPosition;
            if (acc.phase == TouchPhase.Ended && curRecord != Vector2.zero)
            {
                if (Mathf.Abs(curRecord.x) > Mathf.Abs(curRecord.y))
                {
                    if (curRecord.x > 0.0f)
                    {
                        if (mainCharacter.abilityDictionary["DODGE_RIGHT"].initialize_ability())
                        {
                            mainCharacter.curState = "DODGE_RIGHT";
                            mainCharacter.LexhaustScript.instant_thruster(3.0f);
                            mainCharacter.lKneeExhaustScript.instant_thruster(3.5f);
                            mainCharacter.lLegExhaustScript.instant_thruster(3.5f);
                        }
                    }
                    if (curRecord.x < 0.0f)
                    {
                        if (mainCharacter.abilityDictionary["DODGE_LEFT"].initialize_ability())
                        {
                            mainCharacter.curState = "DODGE_LEFT";
                            mainCharacter.RexhaustScript.instant_thruster(3.0f);
                            mainCharacter.rKneeExhaustScript.instant_thruster(3.5f);
                            mainCharacter.rLegExhaustScript.instant_thruster(3.5f);
                        }
                    }
                }
                else
                {
                    mainCharacter.curEnergy -= 10.0f;
                    if (mainCharacter.isClose == true && curRecord.y < 0.0f)
                    {
                        stateChangeTextMod.initialize_text("Chest\nLazer");
                        enable_ability_button(rangeSkillSlots);
                        disable_ability_button(closeSkillSlots);
                        mainCharacter.isClose = false;
                    }
                    else if (mainCharacter.isClose == false && curRecord.y > 0.0f) 
                    {

                        stateChangeTextMod.initialize_text("Blade\nAttack");
                        enable_ability_button(closeSkillSlots);
                        disable_ability_button(rangeSkillSlots);
                        mainCharacter.isClose = true;
                    }
                }
            }
            if (acc.phase == TouchPhase.Ended || acc.phase == TouchPhase.Began)
            {
                curRecord = Vector3.zero;
            }
        }
    }

    void texture_resize(GameObject target, Rect targetSize)
    {
        SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = GUICam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = GUICam.WorldToViewportPoint(targetSprite.bounds.min);
        Vector3 xMax = GUICam.WorldToViewportPoint(targetSprite.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
        //Debug.Log(scaleFactor);
    }


	// Use this for initialization
	void Start () {
        GUICam = this.GetComponent<Camera>();
        

        //skillBottons size&place
        Rect uiButtonSize = new Rect(0.7f, 0.55f, 0.3f, 0.45f);
        texture_resize(lowerRightFrame, uiButtonSize);
        stateChangeTextMod = stateChangeText.GetComponent<UIStringModifier>();
        stateChangeTextMod.initialize_text("Blade\nAttack");

        //Change target button size&place
        uiButtonSize = new Rect(0.0f, 0.55f, 0.3f, 0.45f);
        texture_resize(lowerLeftFrame, uiButtonSize);
        textModifier = changeTargetText.GetComponent<UIStringModifier>();
        textModifier.initialize_text("Change\nTarget");

        //Change hp bar size&place
        uiButtonSize = new Rect(0.0f, 0.0f, 0.3f, 0.15f);
        texture_resize(hpBar, uiButtonSize);


        //Change pause button size&place
        uiButtonSize = new Rect(0.7f, 0.01f, 0.3f, 0.45f);
        texture_resize(upperRightFrame, uiButtonSize);
        textModifier = pauseButtonText.GetComponent<UIStringModifier>();
        textModifier.initialize_text("Pause");

        //Change enemy display
        uiButtonSize = new Rect(0.37f, 0.0f, 0.3f, 0.15f);
        texture_resize(enemyDisplay, uiButtonSize);
        textModifier = enemyDistplayText.GetComponent<UIStringModifier>();

        //Radar resize
        uiButtonSize = new Rect(0.0f, 0.2f, 0.15f, 0.25f);
        texture_resize(radarDisplay, uiButtonSize);


        debuffIconPool = new IList<IconPoolData>[debuffIcons.Length];
        for (int ctr = 0; ctr < debuffIconPool.Length; ctr++)
        {
            IconPoolData temp;
            temp.icon = debuffIcons[ctr];
            temp.isUse = false;
            debuffIconPool[ctr] = new List<IconPoolData>();
            debuffIconPool[ctr].Add(temp);
        }
		endGameScript = endGameWindow.GetComponent<EndBattleLogic>();
		endGameWindow.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        if (isInitialized)
        {
            for (int ctr = 0; ctr < Input.touchCount; ctr++)
            {
                input_commands(Input.GetTouch(ctr));
            }
        }

        if (battleStopped == false)
        {
            float playerHP = 1.0f * mainCharacter.return_cur_stats().baseHp / mainCharacter.return_base_stats().baseHp;
            if (playerHP < 0.0f)
            {
                playerHP = 0.0f;
            }
            playerHpBar.transform.localScale = new Vector3(playerHP, 1.0f, 1.0f);
            if (mainCharacter.target != null)
            {
                textModifier.initialize_text(mainCharacter.target.GetComponent<Character>().characterName);
                Character targetScript = mainCharacter.target.GetComponent<Character>();
                float enemyHP = 1.0f * targetScript.return_cur_stats().baseHp / targetScript.return_base_stats().baseHp;
                if (enemyHP < 0.0f)
                    enemyHP = 0.0f;
                //Debug.Log("target hp: " + targetScript.return_cur_stats().baseHp);
                enemyHealthBar.transform.localScale = new Vector3(enemyHP, 1.0f, 1.0f);
            }
            skillButtons.transform.Rotate(Vector3.forward * Time.deltaTime * 10.0f);
            changeTargetButton.transform.Rotate(Vector3.forward * Time.deltaTime * 10.0f);


            //update energy bar
            if (mainCharacter.gameObject != null)
            {
                int bar = (int)(mainCharacter.energyPercentage * energyBar.Length);
                for (int ctr = 0; ctr < energyBar.Length; ctr++)
                {
                    if (ctr < bar)
                        energyBar[ctr].SetActive(true);
                    else energyBar[ctr].SetActive(false);
                }
            }
            modify_enemy_buff();
        }
	}
}
