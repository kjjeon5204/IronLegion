using UnityEngine;
using System.Collections;

public class MinotaurScript : Character {
	
	public enum CurrentStates {
		IDLE,
		MOVE,
		SHOTGUN,
		MOVESHOTGUN,
		GRENADE,
		MOVEGRENADE
	}
	
	Ability[] abilityList;
	CurrentStates currentStates;
	
	int randomNum;
	float globalCD = 2f;
	float globalCDTracker;
	
	Vector3 heading;
	bool longRange;
	
	int attackStack = 3;
	float moveTime;
	bool movePhasePlayed;
	int movePhaseCtr;
	
	Vector3 exhaustScale;
	public GameObject leftBackExhaust;
	public GameObject rightBackExhaust;
	
	// Use this for initialization
	public override void manual_start () {
		currentStates = CurrentStates.IDLE;
		globalCDTracker = Time.time + globalCD;
		leftBackExhaust.SetActive(false);
		rightBackExhaust.SetActive (false);
		exhaustScale = leftBackExhaust.transform.localScale;
		
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
				moveTime = Time.time + 3.0f;
				movePhasePlayed = false;
				movePhaseCtr++;
			}
		}
	}
	
	void move_phase_two(){
		if (movePhasePlayed == false){
			animation.Play("move");
			transform.Translate(Vector3.forward * 3.0f * Time.deltaTime);
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
	
	void exhaust_check() {
		if (currentStates == CurrentStates.MOVE || currentStates == CurrentStates.MOVESHOTGUN || currentStates == CurrentStates.MOVEGRENADE) {
			leftBackExhaust.SetActive (true);
			rightBackExhaust.SetActive (true);
		} 
		else {
			leftBackExhaust.SetActive  (false);
			rightBackExhaust.SetActive (false);
			leftBackExhaust.transform.localScale = exhaustScale;
			rightBackExhaust.transform.localScale = exhaustScale;
		}
		if (leftBackExhaust.activeSelf == true) {
			leftBackExhaust.transform.localScale += new Vector3(0, 0, .15f * Time.deltaTime);
		}
		if (rightBackExhaust.activeSelf == true) {
			rightBackExhaust.transform.localScale += new Vector3(0, 0, .15f * Time.deltaTime);
		}
	}
	
	
	// Update is called once per frame
	public override void manual_update () {
		//Event Checker
		exhaust_check();
		
		heading = target.transform.position - this.transform.position;
		if (heading.sqrMagnitude < 80f) {
			longRange = false;
		}
		else {	
			longRange = true;
		}
		
		if ((currentStates == CurrentStates.IDLE && globalCDTracker < Time.time)) {
			randomNum = Random.Range (1, 101);
			if (currentStates == CurrentStates.IDLE && attackStack >= 3) {
				if (randomNum >= 0 && randomNum <= 30) {  
					currentStates = CurrentStates.MOVE;
				}
			}
			else if (randomNum >= 30 && randomNum < 90) {
				if (longRange == true) {
					currentStates = CurrentStates.MOVESHOTGUN;
					abilityList[1].initialize_ability();
				}
				else {
					currentStates = CurrentStates.SHOTGUN;
					abilityList[0].initialize_ability();
				}
			}
			else if (randomNum >= 90 && randomNum < 100) {
				if (longRange == true) {
					currentStates = CurrentStates.MOVEGRENADE;
					abilityList[3].initialize_ability();
				}
				else {
					currentStates = CurrentStates.GRENADE;
					abilityList[2].initialize_ability();
				}
			}
			else {
				//wait next frame
			}
			attackStack++;
		}
		
		//Event Handler
		if (currentStates != CurrentStates.MOVE) {
			custom_look_at();
		}
		else if (currentStates == CurrentStates.MOVE) {
			if (movePhaseCtr == 0)
				move_phase_one();
			else if (movePhaseCtr == 1)
				move_phase_two();
			else if (movePhaseCtr == 2) {
				move_phase_three();
				globalCDTracker = Time.time + 3.0f;
			}
		}
		
		if (currentStates == CurrentStates.IDLE) {
			animation.CrossFade ("idle");
		}
		
		else if (currentStates == CurrentStates.SHOTGUN) {
			if (!abilityList[0].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.MOVESHOTGUN) {
			if (!abilityList[1].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		
		else if (currentStates == CurrentStates.GRENADE) {
			if (!abilityList[2].run_ability ()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.MOVEGRENADE) {
			if (!abilityList[3].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
	}
}