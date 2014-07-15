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
    bool phasePlayed;
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

    


    float farDist = 12.0f;
    float closeDist = 8.0f;

    //Dodge
    float dodgeDist;

    public int regAttackCtr = 0;

    public bool turning;

    public GameObject playerCamera;

    //Temporary testing variable

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
        if (worldScript.is_win())
        {
            Destroy(target);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "EnemyAI")
        {
            Character collidedEnemy = collision.collider.gameObject.GetComponent<Character>();
            collidedEnemy.hit(10.0f);
            this.hit(10.0f);
            if (curState == "SWITCHCLOSE")
            {
                isClose = false;
            }
            else if (curState == "SWITCHFAR")
            {
                isClose = true;
            }
            curState = "IDLE";
        }
    }
    
    void line_of_sight_handle()
    {

        GameObject objectLookedAt = this.check_line_of_sight();
        if (objectLookedAt != null) {
            if (objectLookedAt != target) {
                target = objectLookedAt;
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

    void turn_off_effect()
    {
        for (int ctr = 0; ctr < miscEffect.Length; ctr++)
        {
            miscEffect[ctr].SetActive(false);
        }
    }

    //Temporary debug functions
    void temp_input()
    {
        turn_off_effect();
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

    // Use this for initialization
    public override void manual_start()
    {

        curState = "IDLE";
        statData = new HeroStats();
        curLevelData = GetComponent<HeroLevelData>();
        curStats.armor = 0.0f;
        curStats.baseDamage = curLevelData.get_player_stat().damage;
        curStats.baseHp = curLevelData.get_player_stat().HP;
        Debug.Log("Player HP: " + curStats.baseHp);
        baseStats = curStats;

        energyEffect.SetActive(false);
        //Lexhaust.SetActive(false);
        //Rexhaust.SetActive(false);
        //LexhaustReverse.SetActive(false);
        //RexhaustReverse.SetActive(false);


        //Skill name temporary initialization
        abilityNames = new string[8];
        abilityNames[0] = "GATTLING_GUN";
        abilityNames[1] = "SHATTER";
        abilityNames[2] = "BLUTSAUGER";
        abilityNames[3] = "ENERGY_BLADE";
        abilityNames[4] = "SHOTGUN";
        abilityNames[5] = "BARRAGE";
        abilityNames[6] = "AEGIS";
        abilityNames[7] = "BEAM_CANNON";

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

        curEnergy = 100.0f;
        maxEnergy = 100.0f;
        base.manual_start();
	}



    // Update is called once per frame
    public override void manual_update()
    {
        if (worldScript.is_win())
        {
            return;
        }

		currentFlag = targetScript.mapFlag;

        if (target != null)
        {
            currentTargetIndex = targetScript.get_enemy_index();
        }
        if (inputReady == true)
            temp_input();

        if (target == null || targetScript.return_cur_stats().baseHp <= 0)
            get_next_target();
        /*Check Events*/
        float distToTarget = (gameObject.transform.position - target.transform.position).magnitude;
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

        
        if (stateSwitched == true || autoAdjustEnabled == true)
        {
            stateSwitched = false;
			
            float distanceToMove;
            if (isClose == true)
            {
                if (distToTarget < closeDist - 2.0f)
                {
                    //Move away from target
                    curState = "ADJUSTFAR";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = closeDist - distToTarget;
                    movementScript.initialize_movement("BACKWARD", 
                        distanceToMove, 10.0f, Vector3.zero);
                }
                if (distToTarget > closeDist + 2.0f)
                {
                    curState = "ADJUSTCLOSE";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = distToTarget - closeDist;
                    movementScript.initialize_movement("FORWARD",
                        distanceToMove, 10.0f, Vector3.zero);
                }
            }
            else
            {
                if (distToTarget < farDist - 2.0f)
                {
                    curState = "ADJUSTFAR";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = farDist - distToTarget;
                    movementScript.initialize_movement("BACKWARD", distanceToMove, 10.0f, 
                        Vector3.zero);
                }
                if (distToTarget > farDist + 2.0f)
                {
                    curState = "ADJUSTCLOSE";
                    phaseCtr = 0;
                    phasePlayed = false;
                    distanceToMove = distToTarget - farDist;
                    movementScript.initialize_movement("FORWARD", distanceToMove, 10.0f, 
                        Vector3.zero);
                }
            }
            
        }
     

        if (curBattleType == BattleType.REGULAR)
            turning = custom_look_at();
        else if (curBattleType == BattleType.AERIAL)
            turning = custom_look_at_3D(target.transform.position);
        else if (curBattleType == BattleType.BOSS && curState != "PATHING")
            turning = custom_look_at_3D(target.transform.position);


        if (animation.IsPlaying("leftslashstart") && regAttackCtr == 0) {
            regAttackCtr++;
        }

        if (turning == false && curState == "IDLE")
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

            //regAttackCtr = 0;

            //Lexhaust.SetActive(false);
            //Rexhaust.SetActive(false);
            
		}
        else if (curState == "DEATH")
        {

        }
        else if (curState == "ADJUSTFAR" || curState == "ADJUSTCLOSE")
        {
            inputReady = false;
            autoAdjustEnabled = false;
            inputReady = true;
            if (!movementScript.run_movement())
            {
                curState = "IDLE";
                curCharacterState = "IDLE";
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
				currentPath = currentFlag.GetComponent<BattleZone>().
					get_path(targetScript.mapFlag.gameObject);
				currentPath.initialize_path(this.gameObject);
				phasePlayed = true;
			}
			else {
				if (currentPath.run_path()) {
					curState = "IDLE";
				}
			}
        }
        else
        {
            if (!abilityDictionary[curState].run_ability())
            {
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
        previousPos = transform.position;


	}

}
