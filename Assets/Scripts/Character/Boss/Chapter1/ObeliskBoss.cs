using UnityEngine;
using System.Collections;


public class ObeliskBoss : Character {

	public enum CurrentStates
	{
		IDLE,
        MOVE,
		COOLDOWN,
		ROCKET,
		HOWITZER,
		BARRAGE,
		OBLIVION,
		CRUSH,
		MACHINEGUN,
		TRISLASH,
		SONICCHARGE,
		LANCEBEAM,
		LASERWHEEL
	}
	Ability[] abilityList;

	CurrentStates currentStates;
	bool longRange;
	int randomNum;
	int chargeCount = 0;
	
	float missileDelay = .8f;
	float missileDelayTracker;
	
	float chargeCD = .5f;
	float chargeCDTracker;
	
	float globalCD = 1f;
	float globalCDTracker;
	
	float rocketCD = 5; 
	float rocketCDTracker;
	
	float howitzerCD = 5;
	float howitzerCDTracker;
	
	float barrageCD = 15;
	float barrageCDTracker;
	public GameObject[] barrageMuzzle; 
	public GameObject barrageProjectile;
	
	GameObject laserbeam;
	public GameObject laserObject;
	public GameObject laserMuzzle;
	bool lasermade = false;
	
	int phaseCtr = 0;
	bool phasePlayed = false;
	int muzzleCtr = 0;

	Vector3 heading; //for direction/distance vector

    float moveTime;
    int attackStack = 0;
    bool movePhasePlayed;
    int movePhaseCtr;

	
	// Use this for initialization
	public override void manual_start () {
		AIStatElement statHolder = GetComponent<AIStatScript>().getLevelData(1);
        curStats.baseHp = statHolder.hp;
        curStats.baseDamage = statHolder.baseAttack;
        curStats.armor = statHolder.baseArmor;
		baseStats = curStats;
		currentStates = CurrentStates.IDLE;
		targetScript = target.GetComponent<MainChar>();
		longRange = false;
		//Setting attack cooldowns
		globalCDTracker = Time.time + globalCD;
		rocketCDTracker = Time.time + rocketCD;
		howitzerCDTracker = Time.time + howitzerCD;
		barrageCDTracker = Time.time + barrageCD;
		chargeCDTracker = Time.time + chargeCD;
		missileDelayTracker = Time.time + missileDelay;
		abilityList = GetComponents<Ability>();
        GetComponent<Movement>().initialize_script();
		base.manual_start();
	}	
	
	void custom_look_at() {
		float rotAngle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
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
		}
	}
    
	void OnTriggerEnter(Collider hit) {
		if (hit.gameObject.tag != "Boundary" && hit.gameObject.tag != "Projectile"
            && hit.gameObject.tag == "Character") {
			hit.gameObject.GetComponent<Character>().hit (70);
		}
	}


    void move_phase_one()
    {
        if (movePhasePlayed == false)
        {
            animation.Play("movestart");
            movePhasePlayed = true;
        }
        else
        {
            if (!animation.IsPlaying("movestart"))
            {

                moveTime = Time.time + 3.0f;
                movePhasePlayed = false;
                movePhaseCtr++;
            }
        }
    }

    void move_phase_two()
    {
        if (movePhasePlayed == false)
        {
            animation.Play("move");
            transform.Translate(Vector3.forward * 3.0f * Time.deltaTime);
            Vector3 movementVector = find_movement_vector();
            custom_look_at(transform.position + movementVector);
            if (moveTime < Time.time || movementVector == Vector3.zero)
            {
                movePhasePlayed = true;
            }
        }
        else
        {
            movePhasePlayed = false;
            movePhaseCtr++;
        }
    }

    void move_phase_three()
    {
        if (movePhasePlayed == false)
        {
            animation.Play("movestart");
            movePhasePlayed = true;
        }
        else
        {
            if (!animation.IsPlaying("movestart"))
            {
                currentStates = CurrentStates.IDLE;
            }
        }
    }

	//Boss HP at 100-85%
	void stageOne () {
	Debug.Log ("Stage One");
		randomNum = Random.Range (1, 101);
		if (longRange == false) {
			if (randomNum >= 0 && randomNum < 30) {
				currentStates = CurrentStates.CRUSH;
				abilityList[4].initialize_ability();
			}
			else if (randomNum >= 30 && randomNum < 90) {
				currentStates = CurrentStates.MACHINEGUN;
				abilityList[5].initialize_ability();
			}
			else if (randomNum >= 90 && randomNum < 100) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
		}
		else {
			if (randomNum < 35 && rocketCDTracker < Time.time) {
				currentStates = CurrentStates.ROCKET;
				abilityList[0].initialize_ability();
			}
			else if (randomNum >= 35 &&  randomNum < 90 && howitzerCDTracker < Time.time) {
				currentStates = CurrentStates.HOWITZER;
				abilityList[1].initialize_ability();
			}
			else if (randomNum >= 90 && randomNum < 100) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
		}
        attackStack++;
	}
	
	//Boss HP at 85-50%
	void stageTwo () {
		Debug.Log ("Stage Two");
		randomNum = Random.Range (1, 101);
		if (longRange == false) {
			if (randomNum >= 0 && randomNum < 30) {
				currentStates = CurrentStates.CRUSH;
				abilityList[4].initialize_ability();
			}
			else if (randomNum >= 30 && randomNum < 70) {
				currentStates = CurrentStates.MACHINEGUN;
				abilityList[5].initialize_ability();
			}
			else if (randomNum >= 70 && randomNum < 90) {
				currentStates = CurrentStates.SONICCHARGE;
				abilityList[7].initialize_ability();
			}
			else if (randomNum >= 90 && randomNum < 100) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
		}
		else {
			if (randomNum < 35 && rocketCDTracker < Time.time) {
				currentStates = CurrentStates.ROCKET;
				abilityList[0].initialize_ability();
			}
			else if (randomNum >= 35 &&  randomNum < 70 && howitzerCDTracker < Time.time) {
				currentStates = CurrentStates.HOWITZER;
				abilityList[1].initialize_ability();
			}
			else if (randomNum >= 70 && randomNum < 90 && barrageCDTracker < Time.time) {
				currentStates = CurrentStates.BARRAGE;
				abilityList[2].initialize_ability();
			}
			else if (randomNum >= 90 && randomNum < 100) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
		}
        attackStack++;
	}
	
	//Boss HP at 50-0%
	void stageThree () {
		Debug.Log ("Stage Three");
		randomNum = Random.Range (1, 101);
		if (longRange == false) {
			if (randomNum >= 0 && randomNum < 10) {
				currentStates = CurrentStates.CRUSH;
				abilityList[4].initialize_ability();
			}
			else if (randomNum >= 10 && randomNum < 30) {
				currentStates = CurrentStates.MACHINEGUN;
				abilityList[5].initialize_ability();
			}
			else if (randomNum >= 30 && randomNum < 55) {
				currentStates = CurrentStates.SONICCHARGE;
				abilityList[7].initialize_ability();
			}
			else if (randomNum >= 55 && randomNum < 65) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
			else if (randomNum >= 65 && randomNum < 75) {
				currentStates = CurrentStates.LASERWHEEL;
				abilityList[9].initialize_ability();
				lasermade = false;
			}
			else if (randomNum >= 75 && randomNum < 100) {
				currentStates = CurrentStates.TRISLASH;
				abilityList[6].initialize_ability();
			}
		}
		else {
			if (randomNum < 25 && rocketCDTracker < Time.time) {
				currentStates = CurrentStates.ROCKET;
				abilityList[0].initialize_ability();
			}
			else if (randomNum >= 25 &&  randomNum < 40 && howitzerCDTracker < Time.time) {
				currentStates = CurrentStates.HOWITZER;
				abilityList[1].initialize_ability();
			}
			else if (randomNum >= 40 && randomNum < 70 && barrageCDTracker < Time.time) {
				currentStates = CurrentStates.BARRAGE;
				abilityList[2].initialize_ability();
			}
			else if (randomNum >= 70 && randomNum < 75) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
			else if (randomNum >= 75 && randomNum < 85) {
				currentStates = CurrentStates.LASERWHEEL;
				abilityList[9].initialize_ability();
				lasermade = false;
			}
			if (randomNum >= 85 && randomNum < 100) {
				currentStates = CurrentStates.OBLIVION;
				muzzleCtr = 0;
				abilityList[3].initialize_ability();
			}
		}
        attackStack++;
	}
	// Update is called once per frame
	public override void manual_update () {
		//Event Checker
		heading = target.transform.position - this.transform.position;
		if (heading.sqrMagnitude < 100f) {
			longRange = false;
		}
		else {
			longRange = true;
		}
        Debug.Log("Attack stack " + attackStack);
        if ((currentStates == CurrentStates.IDLE && moveTime < Time.time) || 
             (currentStates == CurrentStates.IDLE && attackStack > 3)) {
            int movePoll = Random.Range(0, 4);
            if (movePoll <= attackStack)
            {
                currentStates = CurrentStates.MOVE;
                attackStack = 0;
            }
            moveTime = Time.time + 1.0f;
        }
		
		if ((currentStates == CurrentStates.IDLE && globalCDTracker < Time.time)) {
			if (curStats.baseHp < baseStats.baseHp * .5) {
				stageThree();
			}
			else if (curStats.baseHp < baseStats.baseHp * .85) {
				stageTwo();
			} 
			else {
				stageOne();
			}
			Debug.Log (currentStates);
		}
			/*randomNum = Random.Range(1, 101);
			if (randomNum >= 0 && randomNum < 0) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
			else if (randomNum >= 0 && randomNum < 0) {
				currentStates = CurrentStates.LASERWHEEL;
				abilityList[9].initialize_ability();
				lasermade = false;
			}
			else { //if no default ability is used, use secondary based on range
				randomNum = Random.Range(1, 101);
				if (longRange == false) {				
					if (randomNum >= 0 && randomNum < 0) {
						currentStates = CurrentStates.SONICCHARGE;
						abilityList[7].initialize_ability();
					}
					else if (randomNum >= 0 && randomNum < 0) {
						currentStates = CurrentStates.TRISLASH;
						abilityList[6].initialize_ability();
					}
					else if (randomNum >= 0 && randomNum < 0) {
						currentStates = CurrentStates.MACHINEGUN;
						abilityList[5].initialize_ability();
					}
					else if (randomNum >= 0 && randomNum < 0) {
						currentStates = CurrentStates.CRUSH;
						abilityList[4].initialize_ability();
					}
				}
				else {
                    if (randomNum >= 0 && randomNum < 100 && baseStats.baseHp < baseStats.baseHp * .2) {
						currentStates = CurrentStates.OBLIVION;
						muzzleCtr = 0;
						abilityList[3].initialize_ability();
					}
					else if (randomNum < 0 && rocketCDTracker < Time.time) {
						currentStates = CurrentStates.ROCKET;
						abilityList[0].initialize_ability();
					}
					else if (randomNum >= 0 &&  randomNum < 0 && howitzerCDTracker < Time.time) {
						currentStates = CurrentStates.HOWITZER;
						abilityList[1].initialize_ability();
					}
					else if (randomNum >= 0 && randomNum < 0 && barrageCDTracker < Time.time) {
						currentStates = CurrentStates.BARRAGE;
						abilityList[2].initialize_ability();
					}
					else {
						globalCDTracker = Time.time + globalCD;
					}
				}
			Debug.Log (currentStates);
			}
		}
		*/
	
		//Event Handle
		if (currentStates != CurrentStates.SONICCHARGE && currentStates != CurrentStates.LASERWHEEL) {
			custom_look_at();
		}

		
		if (currentStates == CurrentStates.IDLE) {
			animation.CrossFade ("idle");
		}
		else if (currentStates == CurrentStates.COOLDOWN) {
			if (chargeCDTracker < Time.time) {
				currentStates = CurrentStates.SONICCHARGE;
				abilityList[7].initialize_ability ();
			}
		}
		//LONG RANGE ABILITIES
		else if (currentStates == CurrentStates.ROCKET) {
			rocketCDTracker = Time.time +rocketCD;
			if (!abilityList[0].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.HOWITZER) {
			howitzerCDTracker = Time.time + howitzerCD;
			if (!abilityList[1].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.BARRAGE) {
			barrageCDTracker = Time.time + barrageCD;
			if (!abilityList[2].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}			
		}
		else if (currentStates == CurrentStates.OBLIVION) {
			if (animation.IsPlaying ("castloop")) {
				if (missileDelayTracker < Time.time) {
					GameObject projectileAcc = (GameObject)Instantiate(barrageProjectile, barrageMuzzle[muzzleCtr].transform.position,
					    barrageMuzzle[muzzleCtr].transform.rotation);
					projectileAcc.GetComponent<MyProjectile>().set_projectile(target.GetComponent<MainChar>(), this.gameObject,
					    curStats.baseDamage * 0.4f); 						
					muzzleCtr++;
					missileDelayTracker = Time.time + missileDelay;
				}
			}	
			if (!abilityList[3].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		//CLOSE RANGE ABILITIES
		else if (currentStates == CurrentStates.CRUSH) {
			if (!abilityList[4].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.MACHINEGUN) {
			if (!abilityList[5].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.TRISLASH) {
			if (!abilityList[6].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.SONICCHARGE) {
			if (chargeCount < 3) {
				if (!abilityList[7].run_ability ()) {
					currentStates = CurrentStates.COOLDOWN;
					chargeCount++;
					chargeCDTracker = Time.time + chargeCD;
				}
			}
			else {
				chargeCount = 0;
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		//ANYTIME ABILITIES
		else if (currentStates == CurrentStates.LANCEBEAM) {
			if (!abilityList[8].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.LASERWHEEL) {
			if (animation.IsPlaying("laserlancestart")) {
				transform.Rotate(Vector3.up * 50 * Time.deltaTime);
			}
			if (animation.IsPlaying("laserlanceloop")) {
				if (lasermade == false) {
					laserbeam = (GameObject)Instantiate(laserObject, laserMuzzle.transform.position,
						 this.transform.rotation);
					lasermade = true;
				}
				transform.Rotate(Vector3.down * 20f * Time.deltaTime);
				laserbeam.transform.RotateAround(this.transform.position, Vector3.down, 20f * Time.deltaTime);
			}
			if (!animation.IsPlaying("laserlanceloop")) {
				Destroy(laserbeam);
			}
			if (!abilityList[9].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
        else if (currentStates == CurrentStates.MOVE)
        {
            Debug.Log("Move phase started!");
            if (movePhaseCtr == 0)
                move_phase_one();
            else if (movePhaseCtr == 1)
                move_phase_two();
            else if (movePhaseCtr == 2)
                move_phase_three();
        }
	}
}
