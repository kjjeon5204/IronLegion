using UnityEngine;
using System.Collections;

public class ScorpionBoss : Character {
	
	public enum CurrentStates {
		IDLE,
		SLASH,
		PLASMASHOT,
		BARRAGE
	}
	
	Ability[] abilityList;
	CurrentStates currentStates;
	bool longRange;
	Vector3 heading;
	
	int randomNum;
	float globalCD = 1.0f;
	float globalCDTracker;
	
	// Use this for initialization
	public override void manual_start () {
		currentStates = CurrentStates.IDLE;
		targetScript = target.GetComponent<MainChar>();
		longRange = false;
		
		globalCDTracker = Time.time + globalCD;

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
		Debug.Log ("Player updating");
		//Event Checker
		heading = target.transform.position - this.transform.position;
		if (heading.sqrMagnitude < 100f) {
			longRange = false;
		}
		else {
			longRange = true;
		}
		
		if ((currentStates == CurrentStates.IDLE && globalCDTracker < Time.time)) {
			randomNum = Random.Range (1, 101);
			if (longRange == false) {  //short range abilities
				if (randomNum >= 0 && randomNum < 100) {
					currentStates = CurrentStates.SLASH;
					abilityList[0].initialize_ability();
				}
				else if (randomNum >= 0 && randomNum < 0) {
					currentStates = CurrentStates.PLASMASHOT;
					abilityList[1].initialize_ability ();
				}
			}
			else {                    //long range abilities
				if (randomNum >= 0 && randomNum < 0) {
					currentStates = CurrentStates.BARRAGE;
					abilityList[2].initialize_ability();
				}
				else if (randomNum >= 0 && randomNum < 100) {
					currentStates = CurrentStates.PLASMASHOT;
					abilityList[1].initialize_ability();
				}
			}
			
		}
		
		//Event Handler
		custom_look_at();
		
		if (currentStates == CurrentStates.IDLE) {
			animation.CrossFade ("idle");
		}
		else if (currentStates == CurrentStates.SLASH) {
			if (!abilityList[0].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.PLASMASHOT) {
			if (!abilityList[1].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
		else if (currentStates == CurrentStates.BARRAGE) {
			if (!abilityList[2].run_ability()) {
				currentStates = CurrentStates.IDLE;
				globalCDTracker = Time.time + globalCD;
			}
		}
	}
	
	
	
}
