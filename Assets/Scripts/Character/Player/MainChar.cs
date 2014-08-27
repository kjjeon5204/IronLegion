using UnityEngine;
using System.Collections;
using System.Collections.Generic;





public class MainChar : Character {
    CharSkills skillList;
    string playerState;

    Vector3 previousPos;
    Vector3 initialPos;
    

    void texture_resize(GameObject targetObject, Rect targetSize)
    {
        SpriteRenderer target = targetObject.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, targetSize.center.y, 10.0f);
        target.transform.position = Camera.main.ViewportToWorldPoint(targetPos);
        Vector3 xMin = Camera.main.WorldToViewportPoint(target.bounds.min);
        Vector3 xMax = Camera.main.WorldToViewportPoint(target.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale + 0.05f, yScale + 0.05f, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
    }

    public GameObject zoomCamWindow;

    public CharacterAttackType curAttack;

    //Enemy information
    public Character[] enemyList;
    int currentTargetIndex;

    int phaseCtr;
    int attackFrequency;
    float attackDelay;
    int muzzleTracker;
    public bool phasePlayed;
    public string curState;
    /*
     Player States
     * IDLE
     * DEATH
     * DODGELEEFT
     * DODGERIGHT
     * SWITCHCLOSE
     * SWITCHFAR
     * GATTLING_GUN
     * SHATTER
     * BLUTSAUGER
     * ENERGY_BLADE
     * SHOTGUN
     * BARRAGE
     * AEGIS
     * BEAM_CANNON
    */
    public bool isClose = true;
    bool stateSwitched = false;
    bool autoAdjustEnabled = true;

    HeroStats statData;
    bool inputReady;

    Movement movementScript;
	
    //Effects
    public GameObject energyEffect;
    public GameObject Lexhaust;
    public Booster LexhaustScript;
    public GameObject Rexhaust;
    public Booster RexhaustScript;
    public GameObject LexhaustReverse;
    public Booster RexhaustReverseScript;
    public GameObject RexhaustReverse;
    public Booster LexhaustReverseScript;
    public GameObject lLegExhaust;
    public Booster lLegExhaustScript;
    public GameObject lKneeExhaust;
    public Booster lKneeExhaustScript;
    public GameObject rLegExhaust;
    public Booster rLegExhaustScript;
    public GameObject rKneeExhaust;
    public Booster rKneeExhaustScript;
    public GameObject[] miscEffect;

    //Ability traking variables
    //0 - 3: Close range ability
    //4 - 7: Far range ability
    //The numbers represent the key binding order of the ability
    public string[] abilityNames;
    float[] coolDownTracker;
    Ability[] abilityList;
    public IDictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>();

	MyPathing currentPath;
	MapChargeFlag currentFlag;

    //Event Control
    public GameObject worldObject;
    EventControls worldScript;

    public GameObject targetingIndicator;

    HeroLevelData curLevelData;
    public AudioSource thrusterSound;

    public GameObject rightAllyPositionPoint;
    public GameObject leftAllyPositionPoint;
    BaseAlly allyUnit;

    bool environmentCollision;

    public PlayerCamControls playerCamEffectAccess;
    bool bossApproachPhase = true;
    


    float farDist = 12.0f;
    float closeDist = 8.0f;

    //Dodge
    float dodgeDist;

    public int regAttackCtr = 0;

    public bool turning;

    public GameObject playerCamera;
    public GameObject playerLandingTrackCam;
    public GameObject enemyLandingCamPivotPoint;

    public TargetingIndicator targetIndicatorScript;
    public Camera targetIndicatorCam;

    bool approachPhase = false;
    bool attackPathBuffer = false;

    //Temporary testing variable

    public void target_indicator_switch(bool inputChoice)
    {
        if (inputChoice == true)
            targetingIndicator.SetActive(true);
        else
            targetingIndicator.SetActive(false);
    }

    public void set_ally_unit(BaseAlly inAllyUnit)
    {
        allyUnit = inAllyUnit;
    }

    public bool is_switching_state()
    {
        return stateSwitched;
    }

    public void enable_auto_adjust()
    {
        autoAdjustEnabled = true;
    }

    public void switch_hero_state()
    {
        stateSwitched = true;
    }

    public HeroStats get_hero_stats() {
        return statData;
    }

    public void disable_player_camera()
    {
        if (playerCamera.activeInHierarchy == true)
        {
            playerCamera.SetActive(false);
        }
    }

    public void enable_player_camera()
    {
        if (playerCamera.activeInHierarchy == false)
        {
            playerCamera.SetActive(true);
        }
    }

    public void get_next_target()
    {
        
        if (enemyList.Length == 0) {
            return;
        }
        currentTargetIndex++;
        if (currentTargetIndex >= enemyList.Length)
        {
            currentTargetIndex = 0;
        }
		int loopPreventer = 0;
        while (enemyList[currentTargetIndex].return_cur_stats().baseHp <= 0 &&
		       loopPreventer < enemyList.Length)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= enemyList.Length)
            {
                currentTargetIndex = 0;
            }
			loopPreventer++;
        }
        target = enemyList[currentTargetIndex].gameObject;
        targetScript = enemyList[currentTargetIndex];
        autoAdjustEnabled = true;
        if (curBattleType == BattleType.BOSS)
        {
            attackPathBuffer = true;
            //curState = "PATHING";
            //phasePlayed = false;
        }
        targetIndicatorScript.gameObject.SetActive(true);
        targetIndicatorScript.initialize_indicator();
    }

    public void cancel_player_ability()
    {
        if (abilityDictionary.ContainsKey(curState))
        {
            abilityDictionary[curState].cancel_ability();
        }
    }

    public void reset_player_pos()
    {
        transform.position = initialPos;
    }
   	
	public void set_battle_type(int input) {
		curBattleType = (BattleType)input;
	}

    public bool get_player_state()
    {
        return isClose;
    }

    float calc_damage()
    {
        float damage;
        damage = curAttack.damagePercentage * curStats.baseDamage;
        damage = Random.Range(damage - damage * curAttack.damageRange, damage + damage * curAttack.damageRange);
        return damage;
    }

   
    
    void line_of_sight_handle()
    {
        GameObject objectLookedAt = this.check_line_of_sight();
        if (objectLookedAt != null) {
            if (objectLookedAt != target && curBattleType != BattleType.BOSS) {
                target = objectLookedAt;
				targetScript = objectLookedAt.GetComponent<Character>();
            }
        }
    }
    

    

    
    /*Always eases into looking at a certain direction*/
    
    
    bool custom_look_at() {
        float rotAngle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
        
        if (Mathf.Abs (rotAngle) < 0.04)
        {
            return false;
        }
        float rotDirection = transform.InverseTransformPoint(target.transform.position).x;
    	if (rotAngle > rotSpeed * Time.deltaTime) {
            if (rotDirection > 0)
            {
    			transform.Rotate (Vector3.up * rotSpeed * Time.deltaTime);
    		}
            else if (rotDirection < 0)
            {
    			transform.Rotate (Vector3.up * -rotSpeed * Time.deltaTime);
    		}
    	}
    	else {
            
            if (rotDirection > 0)
                transform.Rotate(Vector3.up * rotAngle);
            else if (rotDirection < 0)
                transform.Rotate(Vector3.down * rotAngle);
             
            //transform.LookAt(target.transform.position);
    	}
        return true;
    }
    
    

    void hit_phase1()
    {
        if (phasePlayed == false)
        {
            animation.Play("flinch");
            phasePlayed = true;
        }
        else
        {
            if (!animation.IsPlaying("flinch")) {
                curState = "IDLE";
            }
        }
    }


    public bool player_input_ready()
    {
        return inputReady;
    }


    public void dodge_message(bool dodgeRight, float dodgeDistIn)
    {
        dodgeDist = dodgeDistIn;
        phasePlayed = false;
        phaseCtr = 0;
        curCharacterState = "DODGE";
        if (dodgeRight == true)
        {
            curState = "DODGERIGHT";
        }
        else
        {
            curState = "DODGELEFT";
        }
    }


    public void event_checker()
    {
        messageReceived = false;
        phaseCtr = 0;
        phasePlayed = false;
        if (curCharacterState == "Turn")
        {
            curState = "TURN";
            curCharacterState = "TURN";
        }
        else if (curCharacterState == "HIT")
        {
            curState = "HIT";
        }
        else if (curCharacterState == "SWITCH")
        {
            if (isClose == true)
            {
                isClose = false;
            }
            else
            {
                isClose = true;
            }
        }
        else
        {
            if (abilityDictionary.ContainsKey(curCharacterState))
                abilityDictionary[curCharacterState].initialize_ability();
        }
    }

    public void turn_off_effect()
    {
        for (int ctr = 0; ctr < miscEffect.Length; ctr++)
        {
            miscEffect[ctr].SetActive(false);
        }
    }

    //Temporary debug functions
    void temp_input()
    {
        //turn_off_effect();
        if (Input.GetKeyDown(KeyCode.Space) && curState != "PATHING")
        { 
            get_next_target();
			if (curBattleType == BattleType.BOSS) {
				curState = "PATHING";
				phasePlayed = false;
			}
        }
        else if (Input.GetKeyDown(KeyCode.G)) 
        {
            if (isClose == true)
                isClose = false;
            else isClose = true;
            stateSwitched = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (isClose == true)
            {
                curState = "GATTLING_GUN";
                abilityDictionary[curState].initialize_ability();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (isClose == true)
            {
                curState = "SHATTER";
                abilityDictionary[curState].initialize_ability();
            }
        }
		else if (Input.GetKeyDown (KeyCode.D)) 
		{
			if (isClose == true) 
			{
				curState = "BLUTSAUGER";
				abilityDictionary[curState].initialize_ability();
			}
		}
		else if (Input.GetKeyDown (KeyCode.F)) 
		{
			if (isClose == true) 
			{
				curState = "ENERGY_BLADE";
				abilityDictionary[curState].initialize_ability();
			}
		}
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isClose == false)
            {
                curState = "SHOTGUN";
                abilityDictionary[curState].initialize_ability();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (isClose == false)
            {
                curState = "BARRAGE";
                abilityDictionary[curState].initialize_ability();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (isClose == false)
            {
                curState = "AEGIS";
                abilityDictionary[curState].initialize_ability();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (isClose == false)
            {
                curState = "BEAM_CANNON";
                abilityDictionary[curState].initialize_ability();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (abilityDictionary["DODGE_RIGHT"].initialize_ability())
            {
                curState = "DODGE_RIGHT";
                LexhaustScript.instant_thruster(3.0f);
                lKneeExhaustScript.instant_thruster(3.5f);
                lLegExhaustScript.instant_thruster(3.5f);
            }
            else
            {
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (abilityDictionary["DODGE_LEFT"].initialize_ability())
            {
                curState = "DODGE_LEFT";
                RexhaustScript.instant_thruster(3.0f);
                rKneeExhaustScript.instant_thruster(3.5f);
                rLegExhaustScript.instant_thruster(3.5f);
            }
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Button held!");
            if (regAttackCtr == 0 && isClose == true && curState != "REGULAR_ATTACK1")
            {
                if (abilityDictionary["REGULAR_ATTACK1"].initialize_ability())
                {
                    curState = "REGULAR_ATTACK1";
                }
            }
            else if (regAttackCtr == 1 && isClose == true)
            {
                if (abilityDictionary["REGULAR_ATTACK2"].initialize_ability())
                {
                    curState = "REGULAR_ATTACK2";
                    regAttackCtr = 2;
                }
            }
            else if (regAttackCtr == 2 && isClose == true)
            {
                if (abilityDictionary["REGULAR_ATTACK3"].initialize_ability())
                {
                    curState = "REGULAR_ATTACK3";
                    regAttackCtr = 0;
                }
            }
            if (isClose == false)
            {
                if (abilityDictionary["SHOULDER_GUN_ATTACK"].initialize_ability()) 
                {
                    curState = "SHOULDER_GUN_ATTACK";
                }
            }
        }
    }

    void OnTriggerEnter(Collider hitCollider)
    {
        Debug.Log("Impact with environment!");
        if (hitCollider.gameObject.tag == "Environment" && environmentCollision == false)
        {
            Vector3 hitDirection = (previousPos - transform.position);
            Debug.Log("Move direction" + hitDirection);
            curState = "IDLE";
            transform.Translate(transform.InverseTransformDirection(hitDirection).normalized * 5.0f);
            playerCamEffectAccess.cam_control_activate("LEFT_RIGHT_SHAKE", 0.3f);
        }
    }




    
    

    void booster_controls()
    {
        Vector3 previousMovement = -transform.InverseTransformPoint(previousPos);
        float totalDist = Mathf.Abs(previousMovement.x) + Mathf.Abs(previousMovement.z);

        LexhaustScript.turn_off_booster();
        RexhaustScript.turn_off_booster();
        LexhaustReverseScript.turn_off_booster();
        RexhaustReverseScript.turn_off_booster();
        lLegExhaustScript.turn_off_booster();
        lKneeExhaustScript.turn_off_booster();
        rLegExhaustScript.turn_off_booster();
        rKneeExhaustScript.turn_off_booster();
        thrusterSound.enabled = true;
        if (previousMovement.z > 0)
        {
            LexhaustScript.turn_on_booster(previousMovement.z / totalDist);
            RexhaustScript.turn_on_booster(previousMovement.z / totalDist);
            lLegExhaustScript.turn_on_booster(previousMovement.z / totalDist);
            lKneeExhaustScript.turn_on_booster(previousMovement.z / totalDist);
            rLegExhaustScript.turn_on_booster(previousMovement.z / totalDist);
            rKneeExhaustScript.turn_on_booster(previousMovement.z / totalDist);
        }
        if (previousMovement.z < 0)
        {
            LexhaustReverseScript.turn_on_booster(-previousMovement.z / totalDist);
            RexhaustReverseScript.turn_on_booster(-previousMovement.z / totalDist);
        }
        if (previousMovement.x > 0)
        {
            LexhaustScript.turn_on_booster(previousMovement.x / totalDist);
            lLegExhaustScript.turn_on_booster(previousMovement.x / totalDist);
            lKneeExhaustScript.turn_on_booster(previousMovement.x / totalDist);
        }
        if (previousMovement.x < 0)
        {
            RexhaustScript.turn_on_booster(-previousMovement.x / totalDist);
            rLegExhaustScript.turn_on_booster(previousMovement.x / totalDist);
            rKneeExhaustScript.turn_on_booster(previousMovement.x / totalDist);
        }
        if (previousMovement.magnitude == 0) {
            thrusterSound.enabled = false;
        }
    }


    public PlayerLevelReadData player_add_experience(int experience)
    {
        PlayerLevelReadData curData;
        curData.levelUp = curLevelData.add_experience(experience);
        curData.expRequired = curLevelData.get_experience_required();
        curData.curExperience = curLevelData.get_player_experience();
        curData.level = curLevelData.get_player_level();
        curLevelData.save_file();
        return curData;
    }

    public void wave_transition_phase(GameObject trackedObject)
    {
        if (playerCamera.activeInHierarchy == true)
        {
            playerCamera.SetActive(false);
        }
        if (playerLandingTrackCam.activeInHierarchy == false)
        {
            playerLandingTrackCam.SetActive(true);
        }
        Vector3 lookAtPointPosition = trackedObject.transform.position;
        lookAtPointPosition.y = 0.0f;
        transform.LookAt(lookAtPointPosition);
        enemyLandingCamPivotPoint.transform.LookAt(trackedObject.transform.position);
    }

    // Use this for initialization
    public override void manual_start()
    {
        
        curState = "IDLE";
        statData = new HeroStats();
        curLevelData = GetComponent<HeroLevelData>();
        HeroStats heroStat = new HeroStats();
        heroStat.load_data();
        PlayerStat playerItemStat = heroStat.get_item_stats();
        curStats.armor = playerItemStat.item_armor;
        curStats.baseDamage = curLevelData.get_player_stat().damage + playerItemStat.item_damage;
        curStats.baseHp = curLevelData.get_player_stat().HP + playerItemStat.item_hp;
        maxEnergy = 100.0f + playerItemStat.item_energy;
        curEnergy = maxEnergy;
        //Debug.Log("Player HP: " + curStats.baseHp);
        baseStats = curStats;

        energyEffect.SetActive(false);
        

        HeroData myAbilityData = new HeroData();
        abilityNames = myAbilityData.load_data();
        
        abilityList = GetComponents<Ability>();

        //Ability Dictionary initialization
        foreach (Ability ability in abilityList)
        {
            abilityDictionary[ability.abilityName] = ability;
        }

        //retrieve movment script
        movementScript = GetComponent<Movement>();
        movementScript.initialize_script();

        worldScript = worldObject.GetComponent<EventControls>();

        LexhaustScript = Lexhaust.GetComponent<Booster>();
        RexhaustScript = Rexhaust.GetComponent<Booster>();
        LexhaustReverseScript = LexhaustReverse.GetComponent<Booster>();
        RexhaustReverseScript = RexhaustReverse.GetComponent<Booster>();
        lLegExhaustScript = lLegExhaust.GetComponent<Booster>();
        lKneeExhaustScript = lKneeExhaust.GetComponent<Booster>();
        rLegExhaustScript = rLegExhaust.GetComponent<Booster>();
        rKneeExhaustScript = rKneeExhaust.GetComponent<Booster>();

        previousPos = transform.position;
        initialPos = transform.position;

        if (curBattleType == BattleType.BOSS)
            approachPhase = true;
        base.manual_start();
	}



    // Update is called once per frame
    public override void manual_update()
    {

        previousPos = transform.position;
		/*
        if (worldScript.is_win())
        {
            return;
        }
		*/
        if (curBattleType == BattleType.BOSS && bossApproachPhase == true)
        {
            transform.Translate(20.0f * Vector3.forward * Time.deltaTime);
        }

        if (allyUnit != null)
        {
            if (target != null)
                allyUnit.set_target(targetScript);
            Vector3 allyRelativePos = transform.InverseTransformPoint(allyUnit.transform.position);
            if (allyRelativePos.x > 0.0f)
            {
                allyUnit.set_movement_position(rightAllyPositionPoint.transform.position);
            }
            else
            {
                allyUnit.set_movement_position(leftAllyPositionPoint.transform.position);
            }
        }

        if (playerCamera.activeInHierarchy == false)
        {
            playerCamera.SetActive(true);
        }
        if (playerLandingTrackCam.activeInHierarchy == true)
        {
            playerLandingTrackCam.SetActive(false);
        }

        float distToTarget = 0;
        
        if (targetScript != null && curState != "PATHING" && attackPathBuffer != true)
            currentFlag = targetScript.mapFlag;
        
        if (inputReady == true && attackPathBuffer == false)
            temp_input();

        if (target == null || targetScript.return_cur_stats().baseHp <= 0)
            get_next_target();
        /*Check Events*/

        if (target != null && targetScript == null)
        {
            target.GetComponent<Character>();
        }

        if (targetScript != null)
        {
            currentTargetIndex = targetScript.get_enemy_index();
            distToTarget = (gameObject.transform.position - target.transform.position).magnitude;

            //Targeting indicator
            float playerTargetAngle = Vector3.Angle(transform.InverseTransformPoint
                (target.transform.position), Vector3.forward);
            
            //if (playerTargetAngle < 8.0f)
            //{
            
            if (targetingIndicator.activeInHierarchy == false)
            {
                targetIndicatorScript.gameObject.SetActive(true);
                targetIndicatorScript.initialize_indicator();
            }
                Vector3 tempPos = playerCamera.GetComponent<Camera>().
                    WorldToViewportPoint(target.collider.bounds.center);
                tempPos = targetIndicatorCam.ViewportToWorldPoint(tempPos);
                targetIndicatorScript.gameObject.transform.position = tempPos;
            //}
            /*
            else
            {
                targetIndicatorScript.gameObject.SetActive(false);
            }
             * */
        }

        //Check if player is facing a valid target
        if (curStats.baseHp <= 0 && curState != "DEATH")
        {
            phasePlayed = false;
            curState = "DEATH";
            curCharacterState = "DEATH";
        }

		if (messageReceived == true && curState == "IDLE") {
			messageReceived = false;
			event_checker();
		}

        float targetDist = 0.0f;
        if ((stateSwitched == true || autoAdjustEnabled == true) && curState == "IDLE" &&
            curState != "PATHING")
        {
            stateSwitched = false;
            if (approachPhase == false)
            {
                //inputReady = false;
                Debug.Log("Input Disabled!");
            }
            else
            {
                inputReady = true;
            }
            float distanceToMove = 0.0f;
            if (isClose == true)
            {
                if (distToTarget < closeDist)
                {
                    //Move away from target
                    curState = "ADJUSTFAR";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = closeDist - distToTarget;
                    Debug.Log("Distance to move: " + distanceToMove);
                    movementScript.initialize_movement("BACKWARD", 
                        distanceToMove, 20.0f, Vector3.zero);
                }
                if (distToTarget > closeDist)
                {
                    curState = "ADJUSTCLOSE";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = distToTarget - closeDist;
                    movementScript.initialize_movement("FORWARD",
                        distanceToMove, 20.0f, Vector3.zero);
                }
                targetDist = distanceToMove;
            }
            else
            {
                if (distToTarget < farDist)
                {
                    curState = "ADJUSTFAR";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = farDist - distToTarget;
                    movementScript.initialize_movement("BACKWARD", distanceToMove, 20.0f, 
                        Vector3.zero);
                }
                if (distToTarget > farDist)
                {
                    curState = "ADJUSTCLOSE";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = distToTarget - farDist;
                    movementScript.initialize_movement("FORWARD", distanceToMove, 20.0f, 
                        Vector3.zero);
                }
                targetDist = distanceToMove;
            }
            
        }
     

        if (curBattleType == BattleType.REGULAR && target != null)
            turning = custom_look_at();
        else if (curBattleType == BattleType.AERIAL && target != null)
            turning = custom_look_at_3D(target.transform.position);
        else if (curBattleType == BattleType.BOSS && curState != "PATHING" && target!= null)
            turning = custom_look_at_3D(target.transform.position);


        if (animation.IsPlaying("leftslashstart") && regAttackCtr == 0) {
            regAttackCtr++;
        }

        if (/*turning == false && */curState == "IDLE")
        {
            line_of_sight_handle();
        }

        //Handles hit
        if (curCharacterState == "Hit")
        {
            curState = "HIT";
        }

        if (curEnergy < maxEnergy)
        {
            curEnergy += Time.deltaTime * 10.0f;
            if (curEnergy > maxEnergy)
            {
                curEnergy = maxEnergy;
            }
        }

        energyPercentage = 1.0f * curEnergy / maxEnergy;




		if (curState == "IDLE") {
            inputReady = true;
			animation.CrossFade ("idle");
			curCharacterState = "IDLE";

		}
        else if (curState == "DEATH")
        {

        }
        else if (curState == "ADJUSTFAR" || curState == "ADJUSTCLOSE")
        {

            if (!movementScript.run_movement() || 
                //Close state distance checkers
                (isClose == true && curState == "ADJUSTFAR" && distToTarget > closeDist) ||
                (isClose == true && curState == "ADJUSTCLOSE" && distToTarget < closeDist) ||
                //Far state distance checkers
                (isClose == false && curState == "ADJUSTFAR" && distToTarget > farDist) ||
                (isClose == false && curState == "ADJUSTCLOSE" && distToTarget < farDist))
            {
                curState = "IDLE";
                curCharacterState = "IDLE";
                autoAdjustEnabled = false;
                approachPhase = false;
                bossApproachPhase = false;
                if (curBattleType == BattleType.BOSS && attackPathBuffer == true)
                {
                    curState = "PATHING";
                    phasePlayed = false;
                }
            }
            //regAttackCtr = 0;
        }
        else if (curState == "HIT")
        {
            inputReady = false;
            if (!animation.IsPlaying(hitAnimation.name))
            {
                curState = "IDLE";
            }
            //regAttackCtr = 0;
        }
        else if (curState == "PATHING")
        {
            if (phasePlayed == false) {
                if (currentFlag == null)
                    Debug.LogError("No flag specified!");
                else if (currentFlag.gameObject == targetScript.mapFlag.gameObject)
                {
                    curState = "IDLE";
                }
                else
                    Debug.Log("Current flag: " + currentFlag.name);
				    currentPath = currentFlag.GetComponent<BattleZone>().
					get_path(targetScript.mapFlag.gameObject);
                if (currentPath != null)
                {
                    currentPath.initialize_path(this.gameObject);
                    phasePlayed = true;
                }
                else
                {
                    autoAdjustEnabled = true;
                    curState = "IDLE";
                    attackPathBuffer = false;
                }
			}
			else {
				if (currentPath.run_path()) {
					curState = "IDLE";
                    attackPathBuffer = false;
				}
			}
        }
        else
        {
            if (!abilityDictionary[curState].run_ability())
            {
                if (curState == "REGULAR_ATTACK1" ||
                    curState == "REGULAR_ATTACK2" ||
                    curState == "REGULAR_ATTACK3" ||
                    curState == "BLUTSAUGER" ||
                    curState == "ENERGY_BLADE" ||
                    curState == "SHATTER")
                {
                    autoAdjustEnabled = true;
                }
                curState = "IDLE";
            }
            if (curState != "IDLE")
                inputReady = abilityDictionary[curState].is_cancellable();
        }
        curCharacterState = curState;
        if (target != null)
        {
            currentTargetIndex = targetScript.get_enemy_index();
        }
        booster_controls();
	}
}