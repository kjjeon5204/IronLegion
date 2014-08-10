using UnityEngine;
using System.Collections;

public class NinjaGreatsword : Character {
	
	public enum CurrentStates {
		IDLE,
		MOVE,
		HITSTUN,
		IMPALE
	}
	
	Ability[] abilityList;
	CurrentStates currentStates;
	
	int randomNum;
	float globalCD = 1.0f;
	float globalCDTracker;
	float moveCD = 3.0f;
	float moveCDTracker;
	
	int attackStack = 0;
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
		moveCDTracker = Time.time + moveCD;
		
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
			animation.Play("moveloop");
			transform.Translate(Vector3.forward * 4.0f * Time.deltaTime);
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
				currentStates = CurrentStates.HITSTUN;
				abilityList[0].initialize_ability();
			}
			else if (randomNum >= 50 && randomNum < 100) {
				currentStates = CurrentStates.IMPALE;
				abilityList[1].initialize_ability ();
			}
			attackStack++;
			Debug.Log(attackStack);
		}
		
		//Event Handler
		if (currentStates != CurrentStates.MOVE) {
			custom_look_at();
		}
		else {
			if (movePhaseCtr == 0)
				move_phase_one();
			else if (movePhaseCtr == 1)
				move_phase_two();
			else if (movePhaseCtr == 2)
				move_phase_three();
		}
		
		if (currentStates == CurrentStates.IDLE) {
			animation.CrossFade ("idle");
		}
		else if (currentStates == CurrentStates.HITSTUN) {
			if (!abilityList[0].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.IMPALE) {
			if (!abilityList[1].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
	}
}