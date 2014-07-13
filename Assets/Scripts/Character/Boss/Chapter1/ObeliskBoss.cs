using UnityEngine;
using System.Collections;


public class ObeliskBoss : Character {

	public enum CurrentStates
	{
		IDLE,
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
	
	float missileDelay = 2.2f;
	float missileDelayTracker;
	
	float chargeCD = .5f;
	float chargeCDTracker;
	
	float globalCD = 2.0f;
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
	
	// Use this for initialization
	public override void manual_start () {
        curStats.baseHp = baseHp;
        curStats.baseDamage = baseDamage;
        curStats.armor = baseArmor;
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
		
		if (currentStates == CurrentStates.IDLE && globalCDTracker < Time.time) {
			randomNum = Random.Range(1, 101);
			if (randomNum >= 0 && randomNum < 30) {
				currentStates = CurrentStates.LANCEBEAM;
				abilityList[8].initialize_ability();
			}
			else if (randomNum >= 30 && randomNum < 50) {
				currentStates = CurrentStates.LASERWHEEL;
				abilityList[9].initialize_ability();
				lasermade = false;
			}
			else { //if no default ability is used, use secondary based on range
				randomNum = Random.Range(1, 101);
				if (longRange == false) {				
					if (randomNum >= 0 && randomNum < 30) {
						currentStates = CurrentStates.SONICCHARGE;
						abilityList[7].initialize_ability();
					}
					else if (randomNum >= 30 && randomNum < 40) {
						currentStates = CurrentStates.TRISLASH;
						abilityList[6].initialize_ability();
					}
					else if (randomNum >= 40 && randomNum < 70) {
						currentStates = CurrentStates.MACHINEGUN;
						abilityList[5].initialize_ability();
					}
					else if (randomNum >= 70 && randomNum < 100) {
						currentStates = CurrentStates.CRUSH;
						abilityList[4].initialize_ability();
					}
				}
				else {
                    if (randomNum >= 0 && randomNum < 20 && baseStats.baseHp < baseStats.baseHp * .2)
                    {
						currentStates = CurrentStates.OBLIVION;
						muzzleCtr = 0;
						abilityList[3].initialize_ability();
					}
					else if (randomNum < 20 && rocketCDTracker < Time.time) {
						currentStates = CurrentStates.ROCKET;
						abilityList[0].initialize_ability();
					}
					else if (randomNum >= 30 &&  randomNum < 80 && howitzerCDTracker < Time.time) {
						currentStates = CurrentStates.HOWITZER;
						abilityList[1].initialize_ability();
					}
					else if (randomNum >= 80 && randomNum < 100 && barrageCDTracker < Time.time) {
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
					laserbeam = (GameObject)Instantiate(laserObject, laserMuzzle.transform.position + this.transform.forward * 8.5f,
						 this.transform.rotation);
					laserbeam.transform.Translate (-.6f, -.3f, 0);
					lasermade = true;
				}
				transform.Rotate(Vector3.down * 15f * Time.deltaTime);
				laserbeam.transform.RotateAround(this.transform.position, Vector3.down, 15f * Time.deltaTime);
			}
			if (!animation.IsPlaying("laserlanceloop")) {
				Destroy(laserbeam);
			}
			if (!abilityList[9].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
	}
}
