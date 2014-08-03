using UnityEngine;
using System.Collections;

public class HopliteAI : Character {
	
	public enum CurrentStates {
		IDLE,
		MOVE,
		SLASH,
		SONICCHARGE,
		COOLDOWN
	}
	
	Ability[] abilityList;
	CurrentStates currentStates;
	
	int randomNum;
	float globalCD = .5f;
	float globalCDTracker;

	float chargeCD = 1.2f;
	float chargeCDTracker;
	int chargeCount = 0;

	int attackStack = 3;
	float moveTime;
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
		globalCDTracker = Time.time + globalCD;
		chargeCDTracker = Time.time + chargeCD;

		abilityList = GetComponents<Ability>();
		GetComponent<Movement>().initialize_script();
		base.manual_start();
	}
	
	void OnTriggerEnter(Collider hit) {
		if (hit.gameObject.tag != "Boundary" && hit.gameObject.tag != "Projectile"
		    && hit.gameObject.tag == "Character" && currentStates == CurrentStates.SONICCHARGE) {
			hit.gameObject.GetComponent<Character>().hit (30);
		}
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
	
	void move_phase_one() {
		if (movePhasePlayed == false) {
			animation.Play("movestart");
			movePhasePlayed = true;
		}
		else {
			if (!animation.IsPlaying("movestart")) {
				moveTime = Time.time + 1.5f;
				movePhasePlayed = false;
				movePhaseCtr++;
			}
		}
	}
	
	void move_phase_two(){
		if (movePhasePlayed == false){
			animation.Play("move");
			animation["move"].speed = 3.0f;
			transform.Translate(Vector3.forward * 2.0f * Time.deltaTime);
			Vector3 movementVector = find_movement_vector();
			custom_look_at(transform.position + movementVector);
			if (moveTime < Time.time || movementVector == Vector3.zero) {
				movePhasePlayed = true;
			}
		}
		else {
			movePhasePlayed = false;
			movePhaseCtr++;
		}
	}
	
	void move_phase_three() {
		if (movePhasePlayed == false) {
			animation.Play("movestop");
			movePhasePlayed = true;
		}
		else {
			if (!animation.IsPlaying("movestop")) {
				currentStates = CurrentStates.IDLE;
				movePhasePlayed = false;
				movePhaseCtr = 0;
				attackStack = 0;
			}
		}
	}
	
	
	// Update is called once per frame
	public override void manual_update () {
		//Event Checker
		if (currentStates == CurrentStates.IDLE && attackStack >= 3) {
			currentStates = CurrentStates.MOVE;
		}
		
		if ((currentStates == CurrentStates.IDLE && globalCDTracker < Time.time)) {
			randomNum = Random.Range (1, 101);
			if (randomNum >= 0 && randomNum < 50) {
				currentStates = CurrentStates.SLASH;
				abilityList[0].initialize_ability();
			}
			else if (randomNum >= 50 && randomNum < 100) {
				currentStates = CurrentStates.SONICCHARGE;
				abilityList[1].initialize_ability();
			}
			attackStack++;
			Debug.Log(attackStack);
		}
		
		//Event Handler
		if (currentStates != CurrentStates.MOVE && currentStates != CurrentStates.SONICCHARGE) {
			custom_look_at();
		}
		else if (currentStates == CurrentStates.MOVE) {
			if (movePhaseCtr == 0)
				move_phase_one();
			else if (movePhaseCtr == 1)
				move_phase_two();
			else if (movePhaseCtr == 2) {
				move_phase_three();
				globalCDTracker = Time.time + 1.5f;
			}
		}
		
		if (currentStates == CurrentStates.IDLE) {
			animation.CrossFade ("idle");
		}
		
		else if (currentStates == CurrentStates.SLASH) {
			if (!abilityList[0].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		
		else if (currentStates == CurrentStates.SONICCHARGE) {
			if (chargeCount < 2) {
				if (!abilityList[1].run_ability ()) {
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
		else if (currentStates == CurrentStates.COOLDOWN) {
			animation.CrossFade("idle");
			if (chargeCDTracker < Time.time) {
				currentStates = CurrentStates.SONICCHARGE;
				abilityList[1].initialize_ability ();
			}
		}
	}
}