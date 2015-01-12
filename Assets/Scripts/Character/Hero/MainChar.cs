using UnityEngine;
using System.Collections;
using System.Collections.Generic;





public class MainChar : Character
{
    public bool tutorialMech = false;
    public PlayerMasterData playerMasterData;

    CharSkills skillList;
    string playerState;

    Vector3 previousPos;
    Vector3 initialPos;

    public struct CancelStateStatus
    {
        public bool attackAvailable;
        public bool dodgeAvailable;
    }

    CancelStateStatus curCancelStatus;


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
    public MapChargeFlag currentFlag;

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
    public GameObject environmentCam;
    public Camera targetIndicatorCam;
    public GameObject playerLandingTrackCam;
    public GameObject enemyLandingCamPivotPoint;

    public TargetingIndicator targetIndicatorScript;

    bool approachPhase = false;
    bool attackPathBuffer = false;
    public bool initiateDeathSequence = false;
    public bool deathSequenceFinished = false;
    public Renderer playerMeshRenderer;

    void Update()
    {
        if (initiateDeathSequence == true)
        {
            if (!animation.IsPlaying("deathstart") && !animation.IsPlaying("deathloop") && 
                deathSequenceFinished == false)
            {
                targetingIndicator.SetActive(false);
                LexhaustScript.shut_down_booster();
                RexhaustScript.shut_down_booster();
                RexhaustReverseScript.shut_down_booster();
                LexhaustReverseScript.shut_down_booster();
                lLegExhaustScript.shut_down_booster();
                rLegExhaustScript.shut_down_booster();
                lKneeExhaustScript.shut_down_booster();
                rKneeExhaustScript.shut_down_booster();
                Instantiate(detonatorDeath, gameObject.transform.position, gameObject.transform.rotation);
                playerMeshRenderer.enabled = false;
                deathSequenceFinished = true;
            }
        }
    }

    public void disable_targeting_indicator()
    {
        targetingIndicator.SetActive(false);
    }

    //Temporary testing variable
    public void cancel_cur_state()
    {
        if (curState != "IDLE" && curState != "DEATH" && curState != "PATHING"
            && curState != "HIT" && curState != "ADJUSTFAR" && curState != "ADJUSTCLOSE")
        {
            if (abilityDictionary.ContainsKey(curState))
            {
                abilityDictionary[curState].cancel_ability();
                //Debug.Log("Ability canceled: " + curState);
            }
            curState = "IDLE";
        }
    }

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

    public HeroStats get_hero_stats()
    {
        return statData;
    }

    public void disable_player_camera()
    {
        if (playerCamera.activeInHierarchy == true)
        {
            playerCamera.SetActive(false);
            environmentCam.SetActive(false);
            targetIndicatorCam.gameObject.SetActive(false);
        }
    }

    public void enable_player_camera()
    {
        if (playerCamera.activeInHierarchy == false)
        {
            playerCamera.SetActive(true);
            environmentCam.SetActive(true);
            targetIndicatorCam.gameObject.SetActive(true);
        }
    }

    public void get_next_target()
    {

        if (enemyList.Length == 0)
        {
            return;
        }
        currentTargetIndex++;
        if (currentTargetIndex >= enemyList.Length)
        {
            currentTargetIndex = 0;
        }
        int loopPreventer = 0;
        while (enemyList[currentTargetIndex].return_cur_stats().hp <= 0 &&
               loopPreventer <= enemyList.Length)
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
        }
        targetIndicatorScript.gameObject.SetActive(true);
        targetIndicatorScript.initialize_indicator();
    }

    public GameObject find_nearest_enemy()
    {
        if (baseCombatStructure != null)
        {
            IList<Character> tempEnemyList = baseCombatStructure.get_list_of_enemy();
            enemyList = new Character[tempEnemyList.Count];
            tempEnemyList.CopyTo(enemyList, 0);
        }
        float curMaxDist = 10000;
        GameObject currentNearest = null;
        for (int ctr = 0; ctr < enemyList.Length; ctr++)
        {
            if (enemyList[ctr].return_cur_stats().hp > 0)
            {
                float distance = (transform.position - enemyList[ctr].transform.position).magnitude;
                if (distance < curMaxDist)
                {
                    curMaxDist = distance;
                    currentNearest = enemyList[ctr].gameObject;
                }
            }
        }
        if (curBattleType == BattleType.BOSS)
        {
            attackPathBuffer = true;
        }
        return currentNearest;
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

    public void set_battle_type(int input)
    {
        curBattleType = (BattleType)input;
    }

    public bool get_player_state()
    {
        return isClose;
    }

    float calc_damage()
    {
        float damage;
        damage = curAttack.damagePercentage * curStats.damage;
        damage = Random.Range(damage - damage * curAttack.damageRange, damage + damage * curAttack.damageRange);
        return damage;
    }



    void line_of_sight_handle()
    {
        GameObject objectLookedAt = this.check_line_of_sight();
        if (objectLookedAt != null)
        {
            if (objectLookedAt != target && curBattleType != BattleType.BOSS)
            {
                target = objectLookedAt;
                targetScript = objectLookedAt.GetComponent<Character>();
            }
        }
    }





    /*Always eases into looking at a certain direction*/


    bool custom_look_at()
    {
        float rotAngle = Vector3.Angle(transform.forward, target.transform.position - transform.position);

        if (Mathf.Abs(rotAngle) < 0.04)
        {
            return false;
        }
        float rotDirection = transform.InverseTransformPoint(target.transform.position).x;
        if (rotAngle > rotSpeed * Time.deltaTime)
        {
            if (rotDirection > 0)
            {
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (rotDirection < 0)
            {
                transform.Rotate(Vector3.up * -rotSpeed * Time.deltaTime);
            }
        }
        else
        {

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
            if (!animation.IsPlaying("flinch"))
            {
                curState = "IDLE";
            }
        }
    }


    public CancelStateStatus player_input_ready()
    {
        return curCancelStatus;
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

    

    void OnTriggerEnter(Collider hitCollider)
    {
        //Debug.Log("Collided with " + hitCollider.gameObject);
        if (hitCollider.gameObject.tag == "Environment")
        {

            //Debug.Log("Impact with environment!");
            Vector3 hitDirection = (previousPos - transform.position);
            //Debug.Log("Move direction" + hitDirection);
            if (attackPathBuffer == false) 
                curState = "IDLE";
            hitDirection = transform.InverseTransformDirection(hitDirection);
            transform.Translate(hitDirection);
            //transform.position = previousPos;
            playerCamEffectAccess.cam_control_activate("LEFT_RIGHT_SHAKE", 0.1f);
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
        if (previousMovement.magnitude == 0)
        {
        }
    }


    public PlayerLevelReadData player_add_experience(int experience)
    {
        PlayerLevelReadData curData;
        curData.levelUp = curLevelData.add_experience(experience);
        curData.expRequired = curLevelData.get_experience_required();
        curData.curExperience = curLevelData.get_player_experience();
        curData.level = curLevelData.get_player_level();
        curLevelData.save_data();
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
        if (tutorialMech == false)
        {
            statData = new HeroStats();
            curLevelData = GetComponent<HeroLevelData>();
            HeroStats heroStat = new HeroStats();
            curLevelData.playerMasterData = playerMasterData;
            PlayerMasterStat playerMasterStat = curLevelData.get_player_stat_all();
            
            curStats.armor += playerMasterStat.armor;
            curStats.damage += playerMasterStat.damage;
            curStats.hp += playerMasterStat.hp;
           
            maxEnergy += playerMasterStat.energy + 100;
            curStats.energy = (int)maxEnergy;
            curEnergy = maxEnergy;
            baseStats = curStats;
            abilityNames = new string[8];
            abilityNames[0] = "GATTLING_GUN";
            abilityNames[1] = "SHATTER";
            abilityNames[4] = "SHOTGUN";
            abilityNames[5] = "BARRAGE";
            if (playerMasterStat.level >= 5)
            {
                abilityNames[2] = "BLUTSAUGER";
            }
            if (playerMasterStat.level >= 6)
            {
                abilityNames[6] = "AEGIS";
            }
            if (playerMasterStat.level >= 7)
            {
                abilityNames[3] = "ENERGY_BLADE";
            }
            if (playerMasterStat.level >= 8)
            {
                abilityNames[7] = "BEAM_CANNON";
            }
            
        }
        else
        {
            curStats.hp = 500;
            curStats.damage = 100;
            curStats.armor = 0;
            maxEnergy = 100;
            curEnergy = maxEnergy;
            baseStats = curStats;
            abilityNames = new string[8];
            abilityNames[0] = "GATTLING_GUN";
            abilityNames[1] = "SHATTER";
            abilityNames[4] = "SHOTGUN";
            abilityNames[5] = "BARRAGE";
        }

        energyEffect.SetActive(false);

        abilityList = GetComponents<Ability>();

        //Ability Dictionary initialization
        foreach (Ability ability in abilityList)
        {
            abilityDictionary[ability.abilityName] = ability;
        }

        //retrieve movment script
        movementScript = GetComponent<Movement>();
        movementScript.initialize_script();

        if (worldObject != null)
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
        //Debug.Log("Player State: " + curState);
        if (!thrusterSound.isPlaying)
            thrusterSound.Play();
        previousPos = transform.position;

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
        if (playerLandingTrackCam != null && playerLandingTrackCam.activeInHierarchy == true)
        {
            playerLandingTrackCam.SetActive(false);
        }

        float distToTarget = 0;

        if (targetScript != null && curState != "PATHING" && attackPathBuffer != true)
            currentFlag = targetScript.mapFlag;


        
        //if (target == null || targetScript.return_cur_stats().hp <= 0)
            //get_next_target();

        if (target == null || targetScript.return_cur_stats().hp <= 0)
        {
            target = find_nearest_enemy();
            if (target != null)
                targetScript = target.GetComponent<Character>();
        }

        /*Check Events*/

        if (target != null && targetScript == null)
        {
            targetScript = target.GetComponent<Character>();
        }

        if (objectHit == true)
        {
            objectHit = false;
            playerCamEffectAccess.cam_control_activate("LEFT_RIGHT_SHAKE", 0.1f);
        }

        if (target != null)
        {
            currentTargetIndex = targetScript.get_enemy_index();
            distToTarget = (gameObject.transform.position - target.transform.position).magnitude;

            //Targeting indicator
            float playerTargetAngle = Vector3.Angle(transform.InverseTransformPoint
                (target.transform.position), Vector3.forward);

            if (targetingIndicator.activeInHierarchy == false)
            {
                targetIndicatorScript.gameObject.SetActive(true);
                targetIndicatorScript.initialize_indicator();
            }
            Vector3 tempPos = playerCamera.GetComponent<Camera>().
                WorldToViewportPoint(target.collider.bounds.center);
            tempPos = targetIndicatorCam.ViewportToWorldPoint(tempPos);
            targetIndicatorScript.gameObject.transform.position = tempPos;
        }

        //Check if player is facing a valid target
        if (curStats.hp <= 0 && curState != "DEATH")
        {
            phasePlayed = false;
            curState = "DEATH";
            curCharacterState = "DEATH";
        }

        if (messageReceived == true && curState == "IDLE")
        {
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
                //Debug.Log("Input Disabled!");
            }
            else
            {
                curCancelStatus.attackAvailable = false;
                curCancelStatus.dodgeAvailable = true;
                //inputReady = true;
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
                    //Debug.Log("Distance to move: " + distanceToMove);
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
        else if (curBattleType == BattleType.BOSS && curState != "PATHING" && target != null)
            turning = custom_look_at_3D(target.transform.position);


        if (animation.IsPlaying("leftslashstart") && regAttackCtr == 0)
        {
            regAttackCtr++;
        }

        if (target != null && curState == "IDLE")
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
            curEnergy += Time.deltaTime * 6.0f;
            if (curEnergy > maxEnergy)
            {
                curEnergy = maxEnergy;
            }
        }

        energyPercentage = 1.0f * curEnergy / maxEnergy;




        if (curState == "IDLE")
        {
            curCancelStatus.attackAvailable = true;
            curCancelStatus.dodgeAvailable = true;
            //inputReady = true;
            animation.CrossFade("idle");
            curCharacterState = "IDLE";

        }
        else if (curState == "DEATH")
        {

        }
        else if (curState == "ADJUSTFAR" || curState == "ADJUSTCLOSE")
        {
            if (curBattleType == BattleType.AERIAL_MULTI_SPAWN_POINT || curBattleType == BattleType.AERIAL)
            {
                if ((transform.position - target.transform.position).magnitude > 30.0f)
                {
                    transform.Translate(Vector3.forward * 100.0f * Time.deltaTime);
                }
            }
            if (attackPathBuffer == true)
            {
                curCancelStatus.attackAvailable = false;
                curCancelStatus.dodgeAvailable = false;
            }
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
                    curCancelStatus.attackAvailable = false;
                    curCancelStatus.dodgeAvailable = false;
                    phasePlayed = false;
                }
            }
        }
        else if (curState == "HIT")
        {
            curCancelStatus.attackAvailable = false;
            curCancelStatus.dodgeAvailable = false;
            //inputReady = false;
            if (!animation.IsPlaying(hitAnimation.name))
            {
                curState = "IDLE";
            }
            //regAttackCtr = 0;
        }
        else if (curState == "PATHING")
        {
            if (phasePlayed == false)
            {
                if (currentFlag == null)
                    currentFlag = targetScript.mapFlag;
                else if (currentFlag.gameObject == targetScript.mapFlag.gameObject)
                {
                    curState = "IDLE";
                }
                //else
                    //Debug.Log("Current flag: " + currentFlag.name);
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
            else
            {
                if (currentPath.run_path())
                {
                    curState = "IDLE";
                    attackPathBuffer = false;
                }
            }
        }
        else
        {
            //Debug.Log("Player Armor: " + curStats.armor);
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
            {
                curCancelStatus.attackAvailable = abilityDictionary[curState].is_cancellable();
                curCancelStatus.dodgeAvailable = abilityDictionary[curState].is_cancellable();
            }
        }
        curCharacterState = curState;
        if (target != null)
        {
            currentTargetIndex = targetScript.get_enemy_index();
        }
        booster_controls();
    }
}