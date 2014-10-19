using UnityEngine;
using System.Collections;

public class Androktasia : Character {
	
	public enum CurrentStates {
		DROPSHIP,
		IDLE,
		QUADSLASH,
		GLAIVE,
		ORBITINGLASER
	}
	
	Ability[] abilityList;
	CurrentStates currentStates;
	bool longRange;
	Vector3 heading; //used to track range from target
	
	int randomNum;
	public float initialCD = 3.0f;
	float initialCDTracker;
	float globalCD = 1.0f;
	float globalCDTracker;

	bool dronesOut = false;
	public GameObject summonEffect;
	public GameObject drone1, drone2;
	float droneCD = 4f;
	float drone1CDTracker, drone2CDTracker;
	public GameObject projectile;
	public GameObject droneMuzzle1, droneMuzzle2;
	
	bool ultimateUsed = false;
	bool objectMade = false;
	public GameObject caution;
	public GameObject damageObject;
	public GameObject laser;
	public GameObject laserWarning;
	public float laserRate;
	float laserRateTracker;
	public float laserDelay;
	float laserDelayTracker;
	
	
	IEnumerator LaserDelay() {
		Vector3 pastLocation = target.transform.position;
		GameObject laserWarn = (GameObject) Instantiate(laserWarning, pastLocation, Quaternion.Euler (0,0,0));
		yield return new WaitForSeconds(laserDelay);
		GameObject lazer = (GameObject) Instantiate(laser, pastLocation, Quaternion.Euler (0,0,0));
	}
	
	// Use this for initialization
	public override void manual_start () {
		AIStatElement statHolder = GetComponent<AIStatScript>().getLevelData(1);
		curStats.hp = statHolder.hp;
		curStats.damage = (int)statHolder.baseAttack;
		curStats.armor = (int)statHolder.baseArmor;
		baseStats = curStats;
		currentStates = CurrentStates.DROPSHIP;
		targetScript = target.GetComponent<MainChar>();
		longRange = false;
		
		initialCDTracker = Time.time + initialCD;
		globalCDTracker = Time.time + globalCD;
		drone1CDTracker = Time.time + droneCD;
		drone2CDTracker = Time.time + droneCD;
		
		laserRateTracker = Time.time + laserRate;
		laserDelayTracker = Time.time + laserDelay;
		
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
	
	// Update is called once per frame
	public override void manual_update () {
		//Event Checker	
		if (currentStates == CurrentStates.DROPSHIP) {
			animation.Play("fightertomech");
			if (!animation.IsPlaying("fightertomech")){
				currentStates = CurrentStates.IDLE;
			}
		}
		
		heading = target.transform.position - this.transform.position;
		
		if ((currentStates == CurrentStates.IDLE && globalCDTracker < Time.time && initialCDTracker < Time.time)) {
			randomNum = Random.Range (1, 101);

			if (curStats.hp < baseStats.hp * .90f && ultimateUsed == false) {
				currentStates = CurrentStates.ORBITINGLASER;
				GameObject cautionObject = (GameObject)Instantiate(caution, this.transform.position,
				                                                   this.transform.rotation);
				abilityList[2].initialize_ability();
			}
			else if (randomNum >= 0 && randomNum < 40) {
				currentStates = CurrentStates.QUADSLASH;
				abilityList[0].initialize_ability();
			}
			else if (randomNum >= 40 && randomNum < 100) {
				currentStates = CurrentStates.GLAIVE;
				abilityList[1].initialize_ability();
			}
			
			//Check to see if hp is below drone summon threshhold. If it is, summon
			if (curStats.hp < baseStats.hp * .75f && dronesOut == false) {
				drone1.SetActive(true);
				GameObject summon1 = (GameObject) Instantiate (summonEffect, drone1.transform.position, drone1.transform.rotation);
				summon1.transform.parent = drone1.transform;
				Destroy (summon1.gameObject, 1);
				drone1.transform.parent = null;
				
				drone2.SetActive(true);
				GameObject summon2 = (GameObject) Instantiate (summonEffect, drone2.transform.position, drone2.transform.rotation);
				summon2.transform.parent = drone2.transform;
				Destroy (summon2.gameObject, 1);
				drone2.transform.parent = null;
				
				dronesOut = true;
			}
		}
		
		//Event Handler---------------------------------------------------------------------------

		if (currentStates != CurrentStates.QUADSLASH && currentStates != CurrentStates.ORBITINGLASER) {
			custom_look_at();
		}
		
		//Manage drone behavior
		if (dronesOut == true){
			drone1.transform.RotateAround(this.transform.position, Vector3.up, 30f * Time.deltaTime);
			drone1.transform.LookAt(target.transform.position);
			drone2.transform.RotateAround(this.transform.position, Vector3.up, 30f * Time.deltaTime);
			drone2.transform.LookAt(target.transform.position);
			
			if (drone1CDTracker < Time.time) {
				GameObject projectileAcc = (GameObject)Instantiate(projectile, droneMuzzle1.transform.position,
				                                                   droneMuzzle1.transform.rotation);
				projectileAcc.GetComponent<MyProjectile>().set_projectile(target.GetComponent<MainChar>(), this.gameObject,
				                                                          curStats.damage * .1f); 	
				drone1CDTracker = droneCD + Time.time + Random.Range (0, 2); //The drones will shoot every 4 seconds + 0 to 2 seconds
			}
			if (drone2CDTracker < Time.time) {
				GameObject projectileAcc = (GameObject)Instantiate(projectile, droneMuzzle2.transform.position,
				                                                   droneMuzzle2.transform.rotation);
				projectileAcc.GetComponent<MyProjectile>().set_projectile(target.GetComponent<MainChar>(), this.gameObject,
				                                                          curStats.damage * .1f); 	
				drone2CDTracker = droneCD + Time.time + Random.Range (0, 2);
			}
		}
		
		//Manage states
		if (currentStates == CurrentStates.IDLE) {
			animation.CrossFade ("idle");
		}
		else if (currentStates == CurrentStates.QUADSLASH) {
			if ((heading.sqrMagnitude >= 12f  && animation.IsPlaying("slash1fire") == true)
				|| (heading.sqrMagnitude >= 12f && animation.IsPlaying("slash3fire") == true)) {
				custom_look_at ();
				transform.Translate(Vector3.forward * 20.0f * Time.deltaTime);
				
			}
			if (!abilityList[0].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.GLAIVE) {
			if (!abilityList[1].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.ORBITINGLASER) {
			if (animation.IsPlaying("spinningloop")) {
				if (objectMade == false) {
					GameObject dmgObject = (GameObject)Instantiate(damageObject, this.transform.position,
					                                    this.transform.rotation);
					dmgObject.transform.Translate(0, -.2f, 1.5f);
					objectMade = true;
				}
				if (laserRateTracker < Time.time) {
					StartCoroutine(LaserDelay());
					laserRateTracker = Time.time + laserRate;
				}
			}
			
			if (!abilityList[2].run_ability()) {
				currentStates = CurrentStates.IDLE;
				ultimateUsed = true;
				globalCDTracker = Time.time + globalCD;
				objectMade = false;
			}
		}
	}
					
}
